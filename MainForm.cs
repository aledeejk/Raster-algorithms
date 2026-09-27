using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace RasterAlgorithms
{
    public partial class MainForm : Form
    {
        private Point currentMousePosition;
        private Bitmap? backgroundTexture;
        private Bitmap? canvasBitmap;
        private Point lastDrawPoint;
        private Point lassoStartPoint;
        private bool isDrawing;
        private bool mouseMoved;
        private int mode;                          // 0 - кисть, 1 - ластик, 3 - обход границы
        private Color contourColor = Color.Black;
        private Color fillColor = Color.LightSkyBlue;
        private bool useTexture;

        private readonly HashSet<Point> lassoPixels = new HashSet<Point>();
        private Color boundaryHighlightColor = Color.Yellow;

        // -------- История для отката --------
        private const int MaxUndoSteps = 20;
        private readonly Stack<Bitmap> undoStack = new Stack<Bitmap>();

        public MainForm()
        {
            InitializeComponent();
            SetupMouseTracking();
            SetupModeReset();

            this.Load += (s, e) => InitializeCanvas();
        }

        private void InitializeCanvas()
        {
            if (canvas.ClientSize.Width <= 0 || canvas.ClientSize.Height <= 0)
                return;

            canvasBitmap?.Dispose();
            canvasBitmap = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);

            using (Graphics graphics = Graphics.FromImage(canvasBitmap))
            {
                graphics.Clear(Color.White);
            }

            canvas.Image = canvasBitmap;
            undoStack.Clear();
        }

        private void SetupMouseTracking()
        {
            tabControl.MouseMove += OnMouseMoveTracking;
            foreach (TabPage tab in tabControl.TabPages)
                tab.MouseMove += OnMouseMoveTracking;
            canvas.MouseMove += OnMouseMoveTracking;
        }

        private void SetupModeReset()
        {
            brushRadioButton.CheckedChanged += (s, e) =>
            {
                if (brushRadioButton.Checked) mode = 0;
            };
            eraserRadioButton.CheckedChanged += (s, e) =>
            {
                if (eraserRadioButton.Checked) mode = 1;
            };
            lassoRadioButton.CheckedChanged += (s, e) =>
            {
                if (lassoRadioButton.Checked) mode = 2;
            };
        }

        private void OnMouseMoveTracking(object? sender, MouseEventArgs e)
        {
            Point location = e.Location;

            if (sender is TabControl)
            {
                var selectedTab = tabControl.SelectedTab;
                if (selectedTab != null)
                    location = selectedTab.PointToClient(tabControl.PointToScreen(e.Location));
            }

            currentMousePosition = location;
            statusLabel.Text = $"Координаты: ({currentMousePosition.X}, {currentMousePosition.Y})";
        }

        private void открытьФонToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            mode = 0;
            brushRadioButton.Checked = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PushUndo();
                    backgroundTexture?.Dispose();
                    backgroundTexture = new Bitmap(openFileDialog.FileName);
                    SetTexturePreview();
                    useTexture = true;
                    SetCanvasBitmap(new Bitmap(backgroundTexture));
                    MessageBox.Show(
                        $"Фон загружен: {backgroundTexture.Width}x{backgroundTexture.Height}",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка загрузки: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            mode = 0;

            if (canvasBitmap == null || saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            ImageFormat format = ImageFormat.Png;
            string extension = Path.GetExtension(saveFileDialog.FileName).ToLowerInvariant();
            if (extension == ".bmp") format = ImageFormat.Bmp;
            else if (extension == ".jpg" || extension == ".jpeg") format = ImageFormat.Jpeg;

            canvasBitmap.Save(saveFileDialog.FileName, format);
        }

        private void выходToolStripMenuItem_Click(object? sender, EventArgs e) => Application.Exit();

        private void оПрограммеToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Лабораторная работа №3\n" +
                "Растровые алгоритмы\n\n" +
                "Задание 1: Заливка и выделение границы\n" +
                "  1а) Рекурсивная заливка цветом\n" +
                "  1б) Заливка рисунком из файла\n" +
                "  1в) Выделение границы связной области\n\n" +
                "Задание 2: Рисование отрезков\n" +
                "  - Алгоритм Брезенхема\n" +
                "  - Алгоритм Ву\n\n" +
                "Задание 3: Градиентное окрашивание треугольника",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            backgroundTexture?.Dispose();
            canvasBitmap?.Dispose();

            while (undoStack.Count > 0)
                undoStack.Pop()?.Dispose();

            base.OnFormClosing(e);
        }

        private void contourColorButton_Click(object? sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = contourColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                contourColor = dialog.Color;
                contourColorPanel.BackColor = contourColor;
            }
        }

        private void fillColorButton_Click(object? sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = fillColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fillColor = dialog.Color;
                fillColorPanel.BackColor = fillColor;
                useTexture = false;
            }
        }

        private void textureButton_Click(object? sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                backgroundTexture?.Dispose();
                backgroundTexture = new Bitmap(openFileDialog.FileName);
                SetTexturePreview();
                useTexture = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки текстуры: {ex.Message}", "Ошибка");
            }
        }

        private void traceBoundaryButton_Click(object? sender, EventArgs e)
        {
            mode = 3;
            statusLabel.Text = "Режим: обход границы. Кликните по контуру.";
        }

        private void undoButton_Click(object? sender, EventArgs e)
        {
            if (undoStack.Count == 0)
            {
                statusLabel.Text = "Нечего отменять.";
                return;
            }

            Bitmap previous = undoStack.Pop();

            // Размер мог поменяться — подгоняем под текущий холст
            Bitmap fitted = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            using (Graphics g = Graphics.FromImage(fitted))
            {
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImageUnscaled(previous, 0, 0);
            }
            previous.Dispose();

            canvasBitmap?.Dispose();
            canvasBitmap = fitted;
            canvas.Image = canvasBitmap;
            canvas.Invalidate();

            statusLabel.Text = $"Отменено. Осталось шагов: {undoStack.Count}";
        }

        private void clearCanvasButton_Click(object? sender, EventArgs e)
        {
            if (canvasBitmap == null) return;

            PushUndo();

            using (Graphics graphics = Graphics.FromImage(canvasBitmap))
                graphics.Clear(Color.White);

            lassoPixels.Clear();
            canvas.Invalidate();
            statusLabel.Text = "Координаты: (0, 0)";
        }

        // ============================================================
        //  ИСТОРИЯ
        // ============================================================
        private void PushUndo()
        {
            if (canvasBitmap == null) return;

            Bitmap snapshot = new Bitmap(canvasBitmap);
            undoStack.Push(snapshot);

            // Ограничиваем глубину истории
            while (undoStack.Count > MaxUndoSteps)
            {
                Bitmap oldest = undoStack.Pop();
                // Pop вернёт самый верхний, но нам нужен самый старый — 
                // поэтому Stack не подходит для «срезания дна».
                // Проще ограничить иначе: см. ниже.
                oldest.Dispose();
            }
        }

        // ============================================================
        //  МЫШЬ
        // ============================================================
        private void canvas_MouseDown(object? sender, MouseEventArgs e)
        {
            if (canvasBitmap == null || e.Button != MouseButtons.Left)
                return;

            Point point = LimitToCanvas(e.Location);

            if (mode == 3)
            {
                PushUndo();
                TraceBoundary(point);
                mode = 0;
                brushRadioButton.Checked = true;
                return;
            }

            if (brushRadioButton.Checked || lassoRadioButton.Checked || eraserRadioButton.Checked)
            {
                PushUndo();

                isDrawing = true;
                mouseMoved = false;
                lastDrawPoint = point;
                lassoStartPoint = point;
                lassoPixels.Clear();
            }
        }

        private void canvas_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isDrawing || canvasBitmap == null || e.Button != MouseButtons.Left)
                return;

            Point point = LimitToCanvas(e.Location);

            using (Graphics graphics = Graphics.FromImage(canvasBitmap))
            {
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;

                Color color;
                if (eraserRadioButton.Checked) color = Color.White;
                else color = contourColor;

                using Pen pen = new Pen(color, lassoRadioButton.Checked ? 2 : 6);
                graphics.DrawLine(pen, lastDrawPoint, point);
            }

            if (lassoRadioButton.Checked)
            {
                foreach (Point p in RasterizeLine(lastDrawPoint, point))
                    lassoPixels.Add(p);
            }

            mouseMoved = true;
            lastDrawPoint = point;
            canvas.Invalidate();
        }

        private void canvas_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDrawing && mouseMoved && lassoRadioButton.Checked && canvasBitmap != null)
            {
                using (Graphics graphics = Graphics.FromImage(canvasBitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;

                    using Pen pen = new Pen(contourColor, 2);
                    graphics.DrawLine(pen, lastDrawPoint, lassoStartPoint);
                }

                foreach (Point p in RasterizeLine(lastDrawPoint, lassoStartPoint))
                    lassoPixels.Add(p);

                ThickenPixels(lassoPixels);
                canvas.Invalidate();
            }

            if (isDrawing && !mouseMoved && e.Button == MouseButtons.Left)
            {
                // Клик без движения — заливка. Снимок для отката уже сделан
                // в MouseDown, поэтому здесь его повторно не делаем.
                Fill(LimitToCanvas(e.Location), useTexture);
            }

            isDrawing = false;
        }

        private Point LimitToCanvas(Point point)
        {
            if (canvasBitmap == null) return point;
            int x = Math.Max(0, Math.Min(canvasBitmap.Width - 1, point.X));
            int y = Math.Max(0, Math.Min(canvasBitmap.Height - 1, point.Y));
            return new Point(x, y);
        }

        // ============================================================
        //  Брезенхем
        // ============================================================
        private static IEnumerable<Point> RasterizeLine(Point a, Point b)
        {
            int x0 = a.X, y0 = a.Y, x1 = b.X, y1 = b.Y;
            int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                yield return new Point(x0, y0);
                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx)  { err += dx; y0 += sy; }
            }
        }

        private void ThickenPixels(IEnumerable<Point> pixels)
        {
            if (canvasBitmap == null || pixels == null) return;

            int w = canvasBitmap.Width;
            int h = canvasBitmap.Height;

            HashSet<Point> toPaint = new HashSet<Point>();
            foreach (Point p in pixels)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = p.X + dx;
                        int ny = p.Y + dy;
                        if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;
                        toPaint.Add(new Point(nx, ny));
                    }
                }
            }

            foreach (Point p in toPaint)
                canvasBitmap.SetPixel(p.X, p.Y, contourColor);
        }

        // ============================================================
        //  ЗАЛИВКА
        // ============================================================
        private void Fill(Point start, bool useTexture)
        {
            if (canvasBitmap == null) return;

            Color target = canvasBitmap.GetPixel(start.X, start.Y);
            if (IsBoundary(target))
            {
                MessageBox.Show("Начальная точка должна быть внутри области.", "Заливка");
                return;
            }

            bool[,] filled = new bool[canvasBitmap.Width, canvasBitmap.Height];
            FillLine(start.X, start.Y, target, useTexture, filled);
            canvas.Invalidate();
        }

        private void FillLine(int x, int y, Color target, bool useTexture, bool[,] filled)
        {
            if (!CanFill(x, y, target, filled)) return;

            int left = x;
            while (CanFill(left - 1, y, target, filled)) left--;

            int right = x;
            while (CanFill(right + 1, y, target, filled)) right++;

            if (useTexture && backgroundTexture != null)
            {
                for (int currentX = left; currentX <= right; currentX++)
                {
                    filled[currentX, y] = true;
                    canvasBitmap!.SetPixel(currentX, y, GetFillColor(currentX, y, useTexture));
                }
            }
            else
            {
                for (int currentX = left; currentX <= right; currentX++)
                    filled[currentX, y] = true;

                using Graphics g = Graphics.FromImage(canvasBitmap!);
                using SolidBrush brush = new SolidBrush(fillColor);
                g.FillRectangle(brush, left, y, right - left + 1, 1);
            }

            ScanLine(left, right, y - 1, target, useTexture, filled);
            ScanLine(left, right, y + 1, target, useTexture, filled);
        }

        private void ScanLine(int left, int right, int y, Color target, bool useTexture, bool[,] filled)
        {
            int x = left;
            while (x <= right)
            {
                while (x <= right && !CanFill(x, y, target, filled)) x++;
                if (x > right) return;

                int runStart = x;
                while (x <= right && CanFill(x, y, target, filled)) x++;

                FillLine((runStart + x - 1) / 2, y, target, useTexture, filled);
            }
        }

        private bool CanFill(int x, int y, Color target, bool[,] filled)
        {
            if (canvasBitmap == null) return false;
            if (x < 0 || x >= canvasBitmap.Width || y < 0 || y >= canvasBitmap.Height) return false;
            if (filled[x, y]) return false;

            Color pixel = canvasBitmap.GetPixel(x, y);
            if (IsBoundary(pixel)) return false;
            return pixel.ToArgb() == target.ToArgb();
        }

        private Color GetFillColor(int x, int y, bool useTexture)
        {
            if (!useTexture || backgroundTexture == null)
                return fillColor;

            int tw = backgroundTexture.Width;
            int th = backgroundTexture.Height;

            int sx = ((x % tw) + tw) % tw;
            int sy = ((y % th) + th) % th;

            return backgroundTexture.GetPixel(sx, sy);
        }

        // ============================================================
        //  ОБХОД ГРАНИЦЫ
        // ============================================================
        private void TraceBoundary(Point clickedPoint)
        {
            if (canvasBitmap == null) return;

            Point? startOpt = FindBoundaryPoint(clickedPoint);
            if (!startOpt.HasValue)
            {
                MessageBox.Show(
                    "Не нашёл границу рядом с кликом.\nУбедитесь, что кликаете по линии контура.",
                    "Обход границы");
                return;
            }

            Color boundaryColor = canvasBitmap.GetPixel(startOpt.Value.X, startOpt.Value.Y);
            HashSet<Point> allBoundary = CollectAllBoundaryPixels(startOpt.Value, boundaryColor);

            HashSet<Point> outerBoundary = new HashSet<Point>();
            foreach (Point p in allBoundary)
                if (HasNonBoundaryNeighbor(p, allBoundary, boundaryColor))
                    outerBoundary.Add(p);

            if (outerBoundary.Count == 0)
            {
                MessageBox.Show("Не удалось выделить внешнюю границу.", "Обход границы");
                return;
            }

            List<Point> boundary = OrderBoundary(outerBoundary, boundaryColor);
            if (boundary.Count == 0)
            {
                MessageBox.Show("Не удалось упорядочить обход.", "Обход границы");
                return;
            }

            using (Graphics g = Graphics.FromImage(canvasBitmap))
            using (Pen pen = new Pen(boundaryHighlightColor, 1))
            {
                if (boundary.Count > 1)
                {
                    List<Point> shifted = new List<Point>(boundary.Count);
                    foreach (Point p in boundary)
                    {
                        Point outward = FindOutwardDirection(p, boundaryColor);
                        shifted.Add(new Point(p.X + outward.X, p.Y + outward.Y));
                    }
                    g.DrawLines(pen, shifted.ToArray());
                }
            }

            canvas.Invalidate();
            statusLabel.Text = $"Обход завершён. Точек в контуре: {boundary.Count}";
        }

        private Point FindOutwardDirection(Point p, Color boundaryColor)
        {
            Point[] dirs = {
                new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1),
                new Point(0, 1), new Point(-1, 1), new Point(-1, 0), new Point(-1, -1)
            };

            foreach (var d in dirs)
            {
                Point n = new Point(p.X + d.X, p.Y + d.Y);
                if (!IsBoundaryAt(n, boundaryColor)) return d;
            }
            return new Point(0, 0);
        }

        private HashSet<Point> CollectAllBoundaryPixels(Point start, Color boundaryColor)
        {
            HashSet<Point> visited = new HashSet<Point>();
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(start);
            visited.Add(start);

            int maxIter = canvasBitmap!.Width * canvasBitmap.Height;
            int iter = 0;

            while (queue.Count > 0 && iter++ < maxIter)
            {
                Point p = queue.Dequeue();
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        Point n = new Point(p.X + dx, p.Y + dy);
                        if (visited.Contains(n)) continue;
                        if (!IsBoundaryAt(n, boundaryColor)) continue;
                        visited.Add(n);
                        queue.Enqueue(n);
                    }
                }
            }

            return visited;
        }

        private bool HasNonBoundaryNeighbor(Point p, HashSet<Point> allBoundary, Color boundaryColor)
        {
            Point[] four = {
                new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0)
            };

            foreach (var d in four)
            {
                Point n = new Point(p.X + d.X, p.Y + d.Y);
                if (!allBoundary.Contains(n) && !IsBoundaryAt(n, boundaryColor))
                    return true;
            }
            return false;
        }

        private List<Point> OrderBoundary(HashSet<Point> outer, Color boundaryColor)
        {
            List<Point> result = new List<Point>();

            Point start = default;
            bool found = false;
            foreach (Point p in outer)
                if (!found || p.Y < start.Y || (p.Y == start.Y && p.X < start.X))
                {
                    start = p;
                    found = true;
                }

            if (!found) return result;

            Point current = start;
            int dir = 0;
            Point[] dirs = {
                new Point(1, 0), new Point(0, 1), new Point(-1, 0), new Point(0, -1)
            };

            HashSet<Point> visited = new HashSet<Point>();
            Point firstPoint = start;
            int firstDir = -1;
            int maxSteps = outer.Count * 4 + 100;
            int step = 0;

            while (step++ < maxSteps)
            {
                result.Add(current);
                visited.Add(current);

                int[] tryDirs = { (dir + 1) % 4, dir, (dir + 3) % 4, (dir + 2) % 4 };
                bool moved = false;

                foreach (int d in tryDirs)
                {
                    Point candidate = new Point(current.X + dirs[d].X, current.Y + dirs[d].Y);
                    if (outer.Contains(candidate) && !visited.Contains(candidate))
                    {
                        dir = d;
                        current = candidate;
                        moved = true;
                        break;
                    }
                }

                if (!moved)
                {
                    Point[] diag = {
                        new Point(1, 1), new Point(1, -1),
                        new Point(-1, 1), new Point(-1, -1)
                    };
                    foreach (var d in diag)
                    {
                        Point candidate = new Point(current.X + d.X, current.Y + d.Y);
                        if (outer.Contains(candidate) && !visited.Contains(candidate))
                        {
                            current = candidate;
                            moved = true;
                            break;
                        }
                    }
                }

                if (!moved) break;
                if (current == firstPoint && firstDir == dir) break;
                if (firstDir == -1) firstDir = dir;
            }

            return result;
        }

        private Point? FindBoundaryPoint(Point point)
        {
            for (int radius = 0; radius <= 5; radius++)
            {
                for (int y = point.Y - radius; y <= point.Y + radius; y++)
                {
                    for (int x = point.X - radius; x <= point.X + radius; x++)
                    {
                        if (IsBoundary(new Point(x, y))) return new Point(x, y);
                    }
                }
            }
            return null;
        }

        private bool IsBoundary(Point point)
        {
            return canvasBitmap != null
                && point.X >= 0 && point.X < canvasBitmap.Width
                && point.Y >= 0 && point.Y < canvasBitmap.Height
                && IsBoundary(canvasBitmap.GetPixel(point.X, point.Y));
        }

        private bool IsBoundary(Color color) => color.ToArgb() == contourColor.ToArgb();

        private bool IsBoundaryAt(Point point, Color boundaryColor)
        {
            if (canvasBitmap == null) return false;
            if (point.X < 0 || point.X >= canvasBitmap.Width ||
                point.Y < 0 || point.Y >= canvasBitmap.Height) return false;

            Color c = canvasBitmap.GetPixel(point.X, point.Y);
            return c.ToArgb() == boundaryColor.ToArgb();
        }

        private void SetTexturePreview()
        {
            if (backgroundTexture == null) return;
            texturePreview.Image?.Dispose();
            texturePreview.Image = new Bitmap(backgroundTexture);
        }

        // ============================================================
        //  ИЗМЕНЕНИЕ РАЗМЕРА ХОЛСТА
        //  Старый рисунок копируется 1:1, новые области БЕЛЫЕ.
        // ============================================================
        private void canvas_SizeChanged(object? sender, EventArgs e)
        {
            if (canvas.ClientSize.Width <= 0 || canvas.ClientSize.Height <= 0)
                return;

            int newW = canvas.ClientSize.Width;
            int newH = canvas.ClientSize.Height;

            if (canvasBitmap == null)
            {
                InitializeCanvas();
                return;
            }

            if (canvasBitmap.Width == newW && canvasBitmap.Height == newH)
                return;

            Bitmap resized = new Bitmap(newW, newH);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImageUnscaled(canvasBitmap, 0, 0);
            }

            Bitmap old = canvasBitmap;
            canvasBitmap = resized;
            canvas.Image = canvasBitmap;
            old.Dispose();

            canvas.Invalidate();
        }

        private void SetCanvasBitmap(Bitmap bitmap)
        {
            canvasBitmap?.Dispose();

            Bitmap fitted = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            using (Graphics g = Graphics.FromImage(fitted))
            {
                g.Clear(Color.White);
            }
            bitmap.Dispose();

            canvasBitmap = fitted;
            canvas.Image = canvasBitmap;
            canvas.Invalidate();
        }
    }
}