namespace RasterAlgorithms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьФонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel task1ToolsPanel;
        private System.Windows.Forms.RadioButton brushRadioButton;
        private System.Windows.Forms.RadioButton lassoRadioButton;
        private System.Windows.Forms.RadioButton eraserRadioButton;
        private System.Windows.Forms.Button contourColorButton;
        private System.Windows.Forms.Panel contourColorPanel;
        private System.Windows.Forms.Button fillColorButton;
        private System.Windows.Forms.Panel fillColorPanel;
        private System.Windows.Forms.Button textureButton;
        private System.Windows.Forms.PictureBox texturePreview;
        private System.Windows.Forms.Button traceBoundaryButton;
        private System.Windows.Forms.Button clearCanvasButton;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

        // ================================================================
        // Задание 2: отрезки
        private System.Windows.Forms.Panel task2ToolsPanel;
        private System.Windows.Forms.PictureBox lineCanvas;
        private System.Windows.Forms.RadioButton bresenhamRadioButton;
        private System.Windows.Forms.RadioButton wuRadioButton;
        private System.Windows.Forms.Button lineColorButton;
        private System.Windows.Forms.Panel lineColorPanel;
        private System.Windows.Forms.NumericUpDown lineThicknessUpDown;
        private System.Windows.Forms.Label lineThicknessLabel;
        private System.Windows.Forms.Button clearLineCanvasButton;
        private System.Windows.Forms.Label lineInfoLabel;
        // ================================================================

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuStrip = new System.Windows.Forms.MenuStrip();
            файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            открытьФонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            tabControl = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            tabPage3 = new System.Windows.Forms.TabPage();
            task1ToolsPanel = new System.Windows.Forms.Panel();
            brushRadioButton = new System.Windows.Forms.RadioButton();
            lassoRadioButton = new System.Windows.Forms.RadioButton();
            eraserRadioButton = new System.Windows.Forms.RadioButton();
            contourColorButton = new System.Windows.Forms.Button();
            contourColorPanel = new System.Windows.Forms.Panel();
            fillColorButton = new System.Windows.Forms.Button();
            fillColorPanel = new System.Windows.Forms.Panel();
            textureButton = new System.Windows.Forms.Button();
            texturePreview = new System.Windows.Forms.PictureBox();
            traceBoundaryButton = new System.Windows.Forms.Button();
            clearCanvasButton = new System.Windows.Forms.Button();
            undoButton = new System.Windows.Forms.Button();
            canvas = new System.Windows.Forms.PictureBox();

            statusStrip = new System.Windows.Forms.StatusStrip();
            statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            task1ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)texturePreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();

            //
            // menuStrip
            //
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                файлToolStripMenuItem,
                справкаToolStripMenuItem
            });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(1024, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";

            //
            // файлToolStripMenuItem
            //
            файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                открытьФонToolStripMenuItem,
                сохранитьКакToolStripMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                выходToolStripMenuItem
            });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            файлToolStripMenuItem.Text = "Файл";

            //
            // открытьФонToolStripMenuItem
            //
            открытьФонToolStripMenuItem.Name = "открытьФонToolStripMenuItem";
            открытьФонToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            открытьФонToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            открытьФонToolStripMenuItem.Text = "Открыть фон...";
            открытьФонToolStripMenuItem.Click += открытьФонToolStripMenuItem_Click;

            //
            // сохранитьКакToolStripMenuItem
            //
            сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            сохранитьКакToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            сохранитьКакToolStripMenuItem.Click += сохранитьКакToolStripMenuItem_Click;

            //
            // выходToolStripMenuItem
            //
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            выходToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            выходToolStripMenuItem.Text = "Выход";
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;

            //
            // справкаToolStripMenuItem
            //
            справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                оПрограммеToolStripMenuItem
            });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            справкаToolStripMenuItem.Text = "Справка";

            //
            // оПрограммеToolStripMenuItem
            //
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            оПрограммеToolStripMenuItem.Text = "О программе";
            оПрограммеToolStripMenuItem.Click += оПрограммеToolStripMenuItem_Click;

            //
            // tabControl
            //
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(tabPage3);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 24);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1024, 644);
            tabControl.TabIndex = 1;

            //
            // tabPage1
            //
            tabPage1.BackColor = System.Drawing.SystemColors.Control;
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1016, 616);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Задание 1";
            tabPage1.Controls.Add(canvas);
            tabPage1.Controls.Add(task1ToolsPanel);

            //
            // task1ToolsPanel
            //
            task1ToolsPanel.Controls.Add(brushRadioButton);
            task1ToolsPanel.Controls.Add(lassoRadioButton);
            task1ToolsPanel.Controls.Add(eraserRadioButton);
            task1ToolsPanel.Controls.Add(contourColorButton);
            task1ToolsPanel.Controls.Add(contourColorPanel);
            task1ToolsPanel.Controls.Add(fillColorButton);
            task1ToolsPanel.Controls.Add(fillColorPanel);
            task1ToolsPanel.Controls.Add(textureButton);
            task1ToolsPanel.Controls.Add(texturePreview);
            task1ToolsPanel.Controls.Add(traceBoundaryButton);
            task1ToolsPanel.Controls.Add(undoButton);
            task1ToolsPanel.Controls.Add(clearCanvasButton);
            task1ToolsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            task1ToolsPanel.Name = "task1ToolsPanel";
            task1ToolsPanel.Size = new System.Drawing.Size(220, 616);

            //
            // brushRadioButton
            //
            brushRadioButton.AutoSize = true;
            brushRadioButton.Checked = true;
            brushRadioButton.Location = new System.Drawing.Point(12, 14);
            brushRadioButton.Text = "Кисть";

            //
            // eraserRadioButton
            //
            eraserRadioButton.AutoSize = true;
            eraserRadioButton.Location = new System.Drawing.Point(12, 42);
            eraserRadioButton.Text = "Ластик";

            //
            // lassoRadioButton
            //
            lassoRadioButton.AutoSize = true;
            lassoRadioButton.Location = new System.Drawing.Point(12, 70);
            lassoRadioButton.Text = "Лассо (произвольная форма)";

            //
            // contourColorButton
            //
            contourColorButton.Location = new System.Drawing.Point(12, 106);
            contourColorButton.Size = new System.Drawing.Size(145, 28);
            contourColorButton.Text = "Цвет контура";
            contourColorButton.Click += contourColorButton_Click;

            //
            // contourColorPanel
            //
            contourColorPanel.BackColor = System.Drawing.Color.Black;
            contourColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            contourColorPanel.Location = new System.Drawing.Point(166, 110);
            contourColorPanel.Size = new System.Drawing.Size(24, 20);

            //
            // fillColorButton
            //
            fillColorButton.Location = new System.Drawing.Point(12, 142);
            fillColorButton.Size = new System.Drawing.Size(145, 28);
            fillColorButton.Text = "Цвет заливки";
            fillColorButton.Click += fillColorButton_Click;

            //
            // fillColorPanel
            //
            fillColorPanel.BackColor = System.Drawing.Color.LightSkyBlue;
            fillColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            fillColorPanel.Location = new System.Drawing.Point(166, 146);
            fillColorPanel.Size = new System.Drawing.Size(24, 20);

            //
            // textureButton
            //
            textureButton.Location = new System.Drawing.Point(12, 178);
            textureButton.Size = new System.Drawing.Size(178, 28);
            textureButton.Text = "Загрузить текстуру...";
            textureButton.Click += textureButton_Click;

            //
            // texturePreview
            //
            texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            texturePreview.Location = new System.Drawing.Point(12, 214);
            texturePreview.Size = new System.Drawing.Size(80, 80);
            texturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            //
            // traceBoundaryButton
            //
            traceBoundaryButton.Location = new System.Drawing.Point(12, 310);
            traceBoundaryButton.Size = new System.Drawing.Size(178, 30);
            traceBoundaryButton.Text = "Обойти границу";
            traceBoundaryButton.Click += traceBoundaryButton_Click;

            //
            // undoButton
            //
            undoButton.Location = new System.Drawing.Point(12, 348);
            undoButton.Size = new System.Drawing.Size(178, 30);
            undoButton.Text = "Отменить";
            undoButton.Click += undoButton_Click;

            //
            // clearCanvasButton
            //
            clearCanvasButton.Location = new System.Drawing.Point(12, 386);
            clearCanvasButton.Size = new System.Drawing.Size(178, 30);
            clearCanvasButton.Text = "Очистить";
            clearCanvasButton.Click += clearCanvasButton_Click;

            //
            // canvas
            //
            canvas.BackColor = System.Drawing.Color.White;
            canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            canvas.Name = "canvas";
            canvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
            canvas.SizeChanged += canvas_SizeChanged;

            //
            // tabPage2
            //
            tabPage2.BackColor = System.Drawing.SystemColors.Control;
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(1016, 616);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Задание 2";

            // ================================================================
            // Задание 2: панель инструментов
            task2ToolsPanel = new System.Windows.Forms.Panel();
            lineCanvas = new System.Windows.Forms.PictureBox();
            bresenhamRadioButton = new System.Windows.Forms.RadioButton();
            wuRadioButton = new System.Windows.Forms.RadioButton();
            lineColorButton = new System.Windows.Forms.Button();
            lineColorPanel = new System.Windows.Forms.Panel();
            lineThicknessUpDown = new System.Windows.Forms.NumericUpDown();
            lineThicknessLabel = new System.Windows.Forms.Label();
            clearLineCanvasButton = new System.Windows.Forms.Button();
            lineInfoLabel = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)lineCanvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lineThicknessUpDown).BeginInit();
            task2ToolsPanel.SuspendLayout();
            tabPage2.SuspendLayout();

            task2ToolsPanel.Controls.Add(bresenhamRadioButton);
            task2ToolsPanel.Controls.Add(wuRadioButton);
            task2ToolsPanel.Controls.Add(lineColorButton);
            task2ToolsPanel.Controls.Add(lineColorPanel);
            task2ToolsPanel.Controls.Add(lineThicknessLabel);
            task2ToolsPanel.Controls.Add(lineThicknessUpDown);
            task2ToolsPanel.Controls.Add(clearLineCanvasButton);
            task2ToolsPanel.Controls.Add(lineInfoLabel);
            task2ToolsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            task2ToolsPanel.Name = "task2ToolsPanel";
            task2ToolsPanel.Size = new System.Drawing.Size(220, 616);

            // Брезенхем
            bresenhamRadioButton.AutoSize = true;
            bresenhamRadioButton.Checked = true;
            bresenhamRadioButton.Location = new System.Drawing.Point(12, 14);
            bresenhamRadioButton.Text = "Брезенхем (целочисленный)";
            bresenhamRadioButton.Name = "bresenhamRadioButton";

            // Ву
            wuRadioButton.AutoSize = true;
            wuRadioButton.Location = new System.Drawing.Point(12, 42);
            wuRadioButton.Text = "Ву (сглаженный)";
            wuRadioButton.Name = "wuRadioButton";

            // Кнопка выбора цвета
            lineColorButton.Location = new System.Drawing.Point(12, 80);
            lineColorButton.Size = new System.Drawing.Size(145, 28);
            lineColorButton.Text = "Цвет линии";
            lineColorButton.Click += lineColorButton_Click;

            // Превью цвета
            lineColorPanel.BackColor = System.Drawing.Color.Black;
            lineColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lineColorPanel.Location = new System.Drawing.Point(166, 84);
            lineColorPanel.Size = new System.Drawing.Size(24, 20);

            // Толщина
            lineThicknessLabel.AutoSize = true;
            lineThicknessLabel.Location = new System.Drawing.Point(12, 122);
            lineThicknessLabel.Text = "Толщина (для Ву — 1):";

            lineThicknessUpDown.Location = new System.Drawing.Point(12, 144);
            lineThicknessUpDown.Size = new System.Drawing.Size(60, 23);
            lineThicknessUpDown.Minimum = 1;
            lineThicknessUpDown.Maximum = 10;
            lineThicknessUpDown.Value = 1;

            // Очистить
            clearLineCanvasButton.Location = new System.Drawing.Point(12, 185);
            clearLineCanvasButton.Size = new System.Drawing.Size(178, 30);
            clearLineCanvasButton.Text = "Очистить";
            clearLineCanvasButton.Click += clearLineCanvasButton_Click;

            // Инфо
            lineInfoLabel.Location = new System.Drawing.Point(12, 225);
            lineInfoLabel.Size = new System.Drawing.Size(200, 200);
            lineInfoLabel.Text = "ЛКМ — задать начало отрезка.\nЛКМ — задать конец.";
            lineInfoLabel.Name = "lineInfoLabel";

            // Холст для отрезков
            lineCanvas.BackColor = System.Drawing.Color.White;
            lineCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lineCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            lineCanvas.Name = "lineCanvas";
            lineCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            lineCanvas.MouseDown += lineCanvas_MouseDown;
            lineCanvas.SizeChanged += lineCanvas_SizeChanged;

            tabPage2.Controls.Add(lineCanvas);
            tabPage2.Controls.Add(task2ToolsPanel);

            ((System.ComponentModel.ISupportInitialize)lineCanvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)lineThicknessUpDown).EndInit();
            task2ToolsPanel.ResumeLayout(false);
            task2ToolsPanel.PerformLayout();
            tabPage2.ResumeLayout(false);

            // ================================================================

            //
            // tabPage3
            //
            tabPage3.BackColor = System.Drawing.SystemColors.Control;
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(1016, 616);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Задание 3";

            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                statusLabel
            });
            statusStrip.Location = new System.Drawing.Point(0, 668);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1024, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";

            //
            // statusLabel
            //
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(91, 17);
            statusLabel.Text = "Координаты: (0, 0)";

            //
            // openFileDialog
            //
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.bmp;*.jpg;*.jpeg;*.png;*.gif|Все файлы|*.*";
            openFileDialog.Title = "Открыть фон (текстуру)";

            //
            // saveFileDialog
            //
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "PNG изображение|*.png|BMP изображение|*.bmp|JPEG изображение|*.jpg";
            saveFileDialog.Title = "Сохранить как";
            saveFileDialog.DefaultExt = "png";

            //
            // MainForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(1024, 690);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Лабораторная работа №3 - Растровые алгоритмы";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            task1ToolsPanel.ResumeLayout(false);
            task1ToolsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)texturePreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}