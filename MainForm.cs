using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        private int mode;                          // 0 - кисть, 1 - ластик, 2 - лассо, 3 - обход границы
        private Color contourColor = Color.Black;
        private Color fillColor = Color.LightSkyBlue;
        private bool useTexture;

        private readonly HashSet<Point> lassoPixels = new HashSet<Point>();
        private Color boundaryHighlightColor = Color.Red;

        // -------- История для отката --------
        private const int MaxUndoSteps = 20;
        private readonly Stack<Bitmap> undoStack = new Stack<Bitmap>();

        // ================================================================
        // ==== Задание 1в: список упорядоченных граничных точек
        private List<Point> boundaryList = new List<Point>();
        // Классификация: true — левая граница, false — правая
        private Dictionary<Point, bool> boundaryIsLeft = new Dictionary<Point, bool>();
        // ================================================================

        // ================================================================
        // ==== Задание 2: состояние
        private Point? lineStartPoint;         // начало текущего отрезка
        private Color lineColor = Color.Black; // цвет линии
        private int lineThickness = 1;         // толщина
        private Bitmap? lineCanvasBitmap;      // холст                                  

        // Задание 3: градиентный треугольник
        private readonly List<Point> gradientPoints = new List<Point>();
        private readonly List<Color> gradientColors = new List<Color>();
        private Bitmap? gradientCanvasBitmap;
        private readonly Random gradientRand = new Random();

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

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                PushUndo();

                using Bitmap source = new Bitmap(openFileDialog.FileName);

                int w = canvas.ClientSize.Width;
                int h = canvas.ClientSize.Height;

                Bitmap fitted = new Bitmap(w, h);
                using (Graphics g = Graphics.FromImage(fitted))
                {
                    // Белый фон вместо прозрачности
                    g.Clear(Color.White);
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.DrawImageUnscaled(source, 0, 0);
                }

                canvasBitmap?.Dispose();
                canvasBitmap = fitted;
                canvas.Image = canvasBitmap;
                canvas.Invalidate();

                statusLabel.Text = $"Изображение загружено: {source.Width}x{source.Height}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка");
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
                "  1а) Рекурсивная заливка сериями цветом\n" +
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
            boundaryList.Clear();
            boundaryIsLeft.Clear();
            canvas.Invalidate();
            statusLabel.Text = "Координаты: (0, 0)";
        }

        // ============================================================
        // Задание 2: обработчики событий
        private void lineColorButton_Click(object? sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = lineColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                lineColor = dialog.Color;
                lineColorPanel.BackColor = lineColor;
            }
        }

        private void clearLineCanvasButton_Click(object? sender, EventArgs e)
        {
            if (lineCanvasBitmap == null) return;

            using (Graphics g = Graphics.FromImage(lineCanvasBitmap))
                g.Clear(Color.White);

            lineCanvas.Invalidate();
            lineInfoLabel.Text = "ЛКМ — задать начало отрезка.\nЛКМ — задать конец.";
        }
        // ============================================================

        // ============================================================
        //  ИСТОРИЯ
        // ============================================================
        private void PushUndo()
        {
            if (canvasBitmap == null) return;

            Bitmap snapshot = new Bitmap(canvasBitmap);
            undoStack.Push(snapshot);

            // Ограничиваем глубину истории: удаляем самые старые снимки
            if (undoStack.Count > MaxUndoSteps)
            {
                var temp = new Stack<Bitmap>();
                int keep = MaxUndoSteps;
                while (undoStack.Count > 0 && keep-- > 0)
                    temp.Push(undoStack.Pop());
                while (undoStack.Count > 0)
                    undoStack.Pop()?.Dispose();
                while (temp.Count > 0)
                    undoStack.Push(temp.Pop());
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

                // Тонкая линия в 1 пиксель — необходимо для корректного обхода
                using Pen pen = new Pen(color, 1);
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

                    using Pen pen = new Pen(contourColor, 1);
                    graphics.DrawLine(pen, lastDrawPoint, lassoStartPoint);
                }

                foreach (Point p in RasterizeLine(lastDrawPoint, lassoStartPoint))
                    lassoPixels.Add(p);

                canvas.Invalidate();
            }

            if (isDrawing && !mouseMoved && e.Button == MouseButtons.Left)
            {
                // Клик без движения — заливка.
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
        //  Брезенхем (для линии лассо)
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
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        // ============================================================
        //  ЗАЛИВКА (1а, 1б) — рекурсивная заливка сериями по лекции
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
            FillSeries(start.X, start.Y, target, useTexture, filled);
            canvas.Invalidate();
        }

        // Рекурсивная заливка серией:
        //  1) находим [left..right] в строке y,
        //  2) закрашиваем серию,
        //  3) рекурсивно вызываем для КАЖДОЙ точки выше и ниже (буквально по лекции).
        private void FillSeries(int x, int y, Color target, bool useTexture, bool[,] filled)
        {
            if (!CanFill(x, y, target, filled)) return;

            // 1) границы серии
            int left = x;
            while (CanFill(left - 1, y, target, filled)) left--;
            int right = x;
            while (CanFill(right + 1, y, target, filled)) right++;

            // 2) закрашиваем серию [left..right]
            for (int i = left; i <= right; i++)
            {
                filled[i, y] = true;
                canvasBitmap!.SetPixel(i, y, GetFillColor(i, y, useTexture));
            }

            // 3) рекурсия для каждой точки выше и ниже
            for (int i = left; i <= right; i++)
            {
                if (y > 0 && CanFill(i, y - 1, target, filled))
                    FillSeries(i, y - 1, target, useTexture, filled);
                if (y < canvasBitmap!.Height - 1 && CanFill(i, y + 1, target, filled))
                    FillSeries(i, y + 1, target, useTexture, filled);
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

        // 1б) Заливка рисунком:
        //   если текстура МЕНЬШЕ холста — циклически (tile);
        //   если БОЛЬШЕ или равна — берём пиксель по координатам без масштабирования.
        private Color GetFillColor(int x, int y, bool useTexture)
        {
            if (!useTexture || backgroundTexture == null)
                return fillColor;

            int tw = backgroundTexture.Width;
            int th = backgroundTexture.Height;

            int sx, sy;
            if (tw <= canvasBitmap!.Width && th <= canvasBitmap.Height)
            {
                // Маленький файл — циклически
                sx = ((x % tw) + tw) % tw;
                sy = ((y % th) + th) % th;
            }
            else
            {
                // Большой файл — без масштаба, по координатам (с обрезкой)
                sx = Math.Min(x, tw - 1);
                sy = Math.Min(y, th - 1);
            }
            return backgroundTexture.GetPixel(sx, sy);
        }

        // ============================================================
        //  ОБХОД ГРАНИЦЫ (1в) — по спецификации лекции
        // ============================================================
        private void TraceBoundary(Point clickedPoint)
        {
            if (canvasBitmap == null) return;

            // 1) стартовый пиксель рядом с кликом
            Point? startOpt = FindNonWhiteNear(clickedPoint, 8);
            if (!startOpt.HasValue)
            {
                MessageBox.Show("Кликните ближе к контуру.", "Обход границы");
                return;
            }
            Point start = startOpt.Value;

            int W = canvasBitmap.Width;
            int H = canvasBitmap.Height;

            // 2) маска «не-белое» = потенциальная граница
            bool[,] isBorder = new bool[W, H];
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    Color c = canvasBitmap.GetPixel(x, y);
                    if (!(c.R > 230 && c.G > 230 && c.B > 230))
                        isBorder[x, y] = true;
                }

            // 3) 8-связная компонента
            HashSet<Point> component = CollectComponent(start, isBorder);
            if (component.Count < 3)
            {
                MessageBox.Show("Слишком маленькая компонента.", "Обход границы");
                return;
            }

            // 4) «Внутренность справа»: оставляем только пиксели,
            //    у которых есть сосед-не-граница (внутренняя часть).
            //    Лекция: «Если пиксел не соседствует с внутренней частью — не считается граничным».
            HashSet<Point> boundary = ExtractTrueBoundary(component, isBorder);
            if (boundary.Count < 3)
            {
                MessageBox.Show("Не удалось выделить границу.", "Обход границы");
                return;
            }

            // 5) старт — верхний-левый из boundary
            Point traceStart = default;
            bool found = false;
            foreach (Point p in boundary)
                if (!found || p.Y < traceStart.Y || (p.Y == traceStart.Y && p.X < traceStart.X))
                { traceStart = p; found = true; }
            if (!found) { MessageBox.Show("Нет граничных точек.", "Обход границы"); return; }

            // 6) обход по часовой стрелке
            List<Point> ordered = TraceBoundaryOrdered(traceStart, boundary);
            if (ordered.Count < 2)
            {
                MessageBox.Show("Обход не удался.", "Обход границы");
                return;
            }

            // 7) сохраняем список, упорядоченный по y, потом по x (как требует лекция)
            boundaryList = new List<Point>(ordered);
            boundaryList.Sort((a, b) => a.Y != b.Y ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

            // 8) классификация левая/правая:
            //    внутренность справа => левая граница; внутренность слева => правая.
            boundaryIsLeft.Clear();
            for (int i = 0; i < ordered.Count; i++)
            {
                Point cur = ordered[i];
                Point next = ordered[(i + 1) % ordered.Count];

                int dx = Math.Sign(next.X - cur.X);
                int dy = Math.Sign(next.Y - cur.Y);

                // «справа» от направления (dx, dy) в экранных координатах — (dy, -dx)
                int rx = cur.X + dy;
                int ry = cur.Y - dx;

                bool insideRight = rx >= 0 && rx < W && ry >= 0 && ry < H
                                   && !isBorder[rx, ry];
                boundaryIsLeft[cur] = insideRight;
            }

            // 9) рисуем обход красным
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            using (Pen pen = new Pen(boundaryHighlightColor, 2))
            {
                for (int i = 0; i < ordered.Count - 1; i++)
                    if (ordered[i] != ordered[i + 1])
                        g.DrawLine(pen, ordered[i], ordered[i + 1]);
                if (ordered.Count > 1 && ordered[^1] != ordered[0])
                    g.DrawLine(pen, ordered[^1], ordered[0]);
            }

            canvas.Invalidate();
            int leftCount = boundaryIsLeft.Count(kv => kv.Value);
            statusLabel.Text =
                $"Готово. Компонента: {component.Count}, граница: {boundary.Count}, " +
                $"в контуре: {ordered.Count}, левых: {leftCount}, правых: {boundaryIsLeft.Count - leftCount}";
        }

        // Оставляем только пиксели, у которых есть сосед-не-граница (внутренность).
        // Соответствует лекции: «Если пиксел не соседствует с внутренней частью — не считается граничным».
        private HashSet<Point> ExtractTrueBoundary(HashSet<Point> component, bool[,] isBorder)
        {
            var result = new HashSet<Point>();
            int W = canvasBitmap!.Width, H = canvasBitmap.Height;
            foreach (Point p in component)
            {
                bool hasInside = false;
                for (int dx = -1; dx <= 1 && !hasInside; dx++)
                    for (int dy = -1; dy <= 1 && !hasInside; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = p.X + dx, ny = p.Y + dy;
                        if (nx < 0 || nx >= W || ny < 0 || ny >= H) continue;
                        if (!isBorder[nx, ny]) hasInside = true;
                    }
                if (hasInside) result.Add(p);
            }
            return result;
        }

        // Ближайший не-белый пиксель к точке (в радиусе radius).
        private Point? FindNonWhiteNear(Point click, int radius)
        {
            if (canvasBitmap == null) return null;

            for (int r = 0; r <= radius; r++)
            {
                for (int y = click.Y - r; y <= click.Y + r; y++)
                {
                    for (int x = click.X - r; x <= click.X + r; x++)
                    {
                        if (x < 0 || x >= canvasBitmap.Width) continue;
                        if (y < 0 || y >= canvasBitmap.Height) continue;

                        Color c = canvasBitmap.GetPixel(x, y);
                        if (!(c.R > 230 && c.G > 230 && c.B > 230))
                            return new Point(x, y);
                    }
                }
            }
            return null;
        }

        // BFS по маске: собирает 8-связную компоненту isBorder, содержащую start.
        private HashSet<Point> CollectComponent(Point start, bool[,] isBorder)
        {
            var visited = new HashSet<Point>();
            if (canvasBitmap == null) return visited;

            int W = canvasBitmap.Width;
            int H = canvasBitmap.Height;

            if (start.X < 0 || start.X >= W || start.Y < 0 || start.Y >= H) return visited;
            if (!isBorder[start.X, start.Y]) return visited;

            var queue = new Queue<Point>();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = p.X + dx;
                        int ny = p.Y + dy;
                        if (nx < 0 || nx >= W || ny < 0 || ny >= H) continue;
                        if (!isBorder[nx, ny]) continue;
                        Point n = new Point(nx, ny);
                        if (visited.Contains(n)) continue;
                        visited.Add(n);
                        queue.Enqueue(n);
                    }
                }
            }
            return visited;
        }

        // Обход границы по часовой стрелке:
        //  - старт — заданная точка;
        //  - первое направление — вниз (S);
        //  - далее — 90° по часовой стрелке от направления входа;
        //  - поиск следующей — против часовой стрелки;
        //  - остановка — по возврату в старт.
        private List<Point> TraceBoundaryOrdered(Point start, HashSet<Point> component)
        {
            var result = new List<Point>();
            if (component == null || component.Count == 0) return result;
            if (component.Count == 1) { result.Add(start); return result; }

            int[] dx = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };

            bool InComp(int x, int y) => component.Contains(new Point(x, y));

            Point startPoint = start;

            // Первое направление — вниз (индекс 2), поиск против часовой: 2,1,0,7,6,5,4,3
            int firstDir = -1;
            for (int i = 0; i < 8; i++)
            {
                int d = ((2 - i) % 8 + 8) % 8;
                if (InComp(startPoint.X + dx[d], startPoint.Y + dy[d]))
                {
                    firstDir = d;
                    break;
                }
            }

            if (firstDir < 0) { result.Add(startPoint); return result; }

            Point curr = new Point(startPoint.X + dx[firstDir], startPoint.Y + dy[firstDir]);
            int arrivedDir = firstDir;

            result.Add(startPoint);

            int maxSteps = component.Count * 8 + 64;
            int steps = 0;

            while (steps++ < maxSteps)
            {
                // Замыкание — после того, как прошли хотя бы 3 точки
                if (curr == startPoint && result.Count > 2)
                    break;

                result.Add(curr);

                // Следующая — на 90° по часовой от направления входа
                int searchStart = (arrivedDir + 2) % 8;
                int foundDir = -1;
                // Поиск против часовой стрелки
                for (int i = 0; i < 8; i++)
                {
                    int d = ((searchStart - i) % 8 + 8) % 8;
                    if (InComp(curr.X + dx[d], curr.Y + dy[d]))
                    {
                        foundDir = d;
                        break;
                    }
                }

                if (foundDir < 0) break;

                curr = new Point(curr.X + dx[foundDir], curr.Y + dy[foundDir]);
                arrivedDir = foundDir;
            }

            if (result.Count > 1 && result[^1] != result[0])
                result.Add(result[0]);

            return result;
        }

        private bool IsBoundary(Point point)
        {
            return canvasBitmap != null
                && point.X >= 0 && point.X < canvasBitmap.Width
                && point.Y >= 0 && point.Y < canvasBitmap.Height
                && IsBoundary(canvasBitmap.GetPixel(point.X, point.Y));
        }

        private bool IsBoundary(Color color) => color.ToArgb() == contourColor.ToArgb();

        private void SetTexturePreview()
        {
            if (backgroundTexture == null) return;
            texturePreview.Image?.Dispose();
            texturePreview.Image = new Bitmap(backgroundTexture);
        }

        // ============================================================
        //  ИЗМЕНЕНИЕ РАЗМЕРА ХОЛСТА
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

        // ================================================================
        // ==== Задание 2: методы

        // Инициализация холста.
        // Вызывается при первом рисовании и при изменении размера.
        private void EnsureLineCanvasBitmap()
        {
            if (lineCanvas.ClientSize.Width <= 0 || lineCanvas.ClientSize.Height <= 0)
                return;

            int w = lineCanvas.ClientSize.Width;
            int h = lineCanvas.ClientSize.Height;

            if (lineCanvasBitmap != null &&
                lineCanvasBitmap.Width == w &&
                lineCanvasBitmap.Height == h)
                return;

            Bitmap newBmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                g.Clear(Color.White);
                if (lineCanvasBitmap != null)
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.DrawImageUnscaled(lineCanvasBitmap, 0, 0);
                }
            }
            lineCanvasBitmap?.Dispose();
            lineCanvasBitmap = newBmp;
            lineCanvas.Image = lineCanvasBitmap;
        }

        // Обработка клика мышкой по холсту.
        // Первый клик задаёт начало отрезка. Второй клик — конец, отрезок рисуется.
        // Состояние lineStartPoint сбрасывается.
        private void lineCanvas_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            EnsureLineCanvasBitmap();
            if (lineCanvasBitmap == null) return;

            Point p = new Point(
                Math.Max(0, Math.Min(lineCanvasBitmap.Width - 1, e.X)),
                Math.Max(0, Math.Min(lineCanvasBitmap.Height - 1, e.Y)));

            if (lineStartPoint == null)
            {
                // Первый клик — задаём начало
                lineStartPoint = p;
                lineInfoLabel.Text =
                    $"Начало: ({p.X}, {p.Y})\n" +
                    "Кликните второй раз — конец отрезка.";
            }
            else
            {
                // Второй клик — рисуем
                Point start = lineStartPoint.Value;
                Point end = p;

                lineThickness = (int)lineThicknessUpDown.Value;

                if (bresenhamRadioButton.Checked)
                    DrawLineBresenham(start, end, lineColor, lineThickness);
                else
                    DrawLineWu(start, end, lineColor);

                lineStartPoint = null;
                lineCanvas.Invalidate();
                lineInfoLabel.Text =
                    $"Отрезок: ({start.X},{start.Y}) → ({end.X},{end.Y})\n" +
                    $"Алгоритм: {(bresenhamRadioButton.Checked ? "Брезенхем" : "Ву")}";
            }
        }

        // Отображение координат в статус-баре
        private void lineCanvas_MouseMove(object? sender, MouseEventArgs e)
        {
            statusLabel.Text = $"Координаты: ({e.X}, {e.Y})";
        }

        // Изменение размера окна
        private void lineCanvas_SizeChanged(object? sender, EventArgs e)
        {
            EnsureLineCanvasBitmap();
        }

        // Целочисленный алгоритм Брезенхема.
        // Толщина реализуется как квадрат thickness × thickness вокруг каждого пикселя.
        private void DrawLineBresenham(Point a, Point b, Color color, int thickness)
        {
            if (lineCanvasBitmap == null) return;

            int x0 = a.X, y0 = a.Y, x1 = b.X, y1 = b.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = -Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            int half = (thickness - 1) / 2;
            int extra = (thickness - 1) - half;

            while (true)
            {
                // Рисуем "жирный" пиксель
                for (int ox = -half; ox <= extra; ox++)
                {
                    for (int oy = -half; oy <= extra; oy++)
                    {
                        int px = x0 + ox;
                        int py = y0 + oy;
                        if (px < 0 || px >= lineCanvasBitmap.Width) continue;
                        if (py < 0 || py >= lineCanvasBitmap.Height) continue;
                        lineCanvasBitmap.SetPixel(px, py, color);
                    }
                }

                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        // Алгоритм Ву (У Сяолиня) — сглаженная линия.
        // Соседние пиксели рисуются с разной яркостью
        // пропорционально расстоянию до идеальной прямой.
        private void DrawLineWu(Point a, Point b, Color color)
        {
            if (lineCanvasBitmap == null) return;

            int x0 = a.X, y0 = a.Y, x1 = b.X, y1 = b.Y;

            // Если линия более вертикальная, чем горизонтальная —
            // меняем оси, чтобы основная ось была X (иначе сглаживание будет по Y).
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;
            double gradient = dx == 0 ? 1.0 : (double)dy / dx;

            // Первая точка
            double y = y0;
            for (int x = x0; x <= x1; x++)
            {
                int iy = (int)Math.Floor(y);
                double frac = y - iy;

                PlotWu(x, iy, 1.0 - frac, color, steep);
                PlotWu(x, iy + 1, frac, color, steep);

                y += gradient;
            }
        }

        // Рисует один "размазанный" пиксель с заданной интенсивностью.
        private void PlotWu(int x, int y, double intensity, Color color, bool steep)
        {
            if (lineCanvasBitmap == null) return;
            if (intensity <= 0.0) return;
            if (intensity > 1.0) intensity = 1.0;

            int px = steep ? y : x;
            int py = steep ? x : y;

            if (px < 0 || px >= lineCanvasBitmap.Width) return;
            if (py < 0 || py >= lineCanvasBitmap.Height) return;

            // Смешиваем цвет линии с существующим цветом пикселя
            // пропорционально intensity
            Color old = lineCanvasBitmap.GetPixel(px, py);

            int r = (int)(color.R * intensity + old.R * (1.0 - intensity));
            int g = (int)(color.G * intensity + old.G * (1.0 - intensity));
            int bl = (int)(color.B * intensity + old.B * (1.0 - intensity));

            lineCanvasBitmap.SetPixel(px, py, Color.FromArgb(r, g, bl));
        }

        // Задание 3: обработчики для градиентного треугольника
        private void pictureBoxTask3_Click(object? sender, EventArgs e)
        {
            var me = (MouseEventArgs)e;
            if (me.Button != MouseButtons.Left) return;

            if (gradientCanvasBitmap == null
                || gradientCanvasBitmap.Width != pictureBoxTask3.Width
                || gradientCanvasBitmap.Height != pictureBoxTask3.Height)
            {
                gradientCanvasBitmap?.Dispose();
                gradientCanvasBitmap = new Bitmap(pictureBoxTask3.Width, pictureBoxTask3.Height);
                using (Graphics g = Graphics.FromImage(gradientCanvasBitmap))
                {
                    g.Clear(Color.White);
                }
                pictureBoxTask3.Image = gradientCanvasBitmap;
            }

            gradientPoints.Add(me.Location);
            gradientColors.Add(Color.FromArgb(
                gradientRand.Next(256),
                gradientRand.Next(256),
                gradientRand.Next(256)));

            if (gradientPoints.Count == 3)
            {
                using (Graphics g = Graphics.FromImage(gradientCanvasBitmap))
                {
                    GradientTriangle.Draw(g,
                        gradientPoints[0], gradientColors[0],
                        gradientPoints[1], gradientColors[1],
                        gradientPoints[2], gradientColors[2]);
                }
                pictureBoxTask3.Invalidate();
                gradientPoints.Clear();
                gradientColors.Clear();
            }
        }

        private void btnClearTask3_Click(object? sender, EventArgs e)
        {
            gradientPoints.Clear();
            gradientColors.Clear();

            if (gradientCanvasBitmap != null)
            {
                using (Graphics g = Graphics.FromImage(gradientCanvasBitmap))
                {
                    g.Clear(Color.White);
                }
                pictureBoxTask3.Invalidate();
            }
        }
    }
}