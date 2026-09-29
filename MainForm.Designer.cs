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
        private System.Windows.Forms.PictureBox pictureBoxTask3;
        private System.Windows.Forms.Button btnClearTask3;
        private System.Windows.Forms.Label lblHintTask3;
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
            menuStrip = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            открытьФонToolStripMenuItem = new ToolStripMenuItem();
            сохранитьКакToolStripMenuItem = new ToolStripMenuItem();
            выходToolStripMenuItem = new ToolStripMenuItem();
            справкаToolStripMenuItem = new ToolStripMenuItem();
            оПрограммеToolStripMenuItem = new ToolStripMenuItem();
            tabControl = new TabControl();
            tabPage1 = new TabPage();
            canvas = new PictureBox();
            task1ToolsPanel = new Panel();
            brushRadioButton = new RadioButton();
            lassoRadioButton = new RadioButton();
            eraserRadioButton = new RadioButton();
            contourColorButton = new Button();
            contourColorPanel = new Panel();
            fillColorButton = new Button();
            fillColorPanel = new Panel();
            textureButton = new Button();
            texturePreview = new PictureBox();
            traceBoundaryButton = new Button();
            undoButton = new Button();
            clearCanvasButton = new Button();
            tabPage2 = new TabPage();
            lineCanvas = new PictureBox();
            task2ToolsPanel = new Panel();
            bresenhamRadioButton = new RadioButton();
            wuRadioButton = new RadioButton();
            lineColorButton = new Button();
            lineColorPanel = new Panel();
            lineThicknessLabel = new Label();
            lineThicknessUpDown = new NumericUpDown();
            clearLineCanvasButton = new Button();
            lineInfoLabel = new Label();
            tabPage3 = new TabPage();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            pictureBoxTask3 = new PictureBox();
            btnClearTask3 = new Button();
            lblHintTask3 = new Label();
            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            task1ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)texturePreview).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lineCanvas).BeginInit();
            task2ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lineThicknessUpDown).BeginInit();
            tabPage3.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTask3).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, справкаToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1024, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { открытьФонToolStripMenuItem, сохранитьКакToolStripMenuItem, выходToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(59, 24);
            файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьФонToolStripMenuItem
            // 
            открытьФонToolStripMenuItem.Name = "открытьФонToolStripMenuItem";
            открытьФонToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            открытьФонToolStripMenuItem.Size = new Size(251, 26);
            открытьФонToolStripMenuItem.Text = "Открыть фон...";
            открытьФонToolStripMenuItem.Click += открытьФонToolStripMenuItem_Click;
            // 
            // сохранитьКакToolStripMenuItem
            // 
            сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            сохранитьКакToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            сохранитьКакToolStripMenuItem.Size = new Size(251, 26);
            сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            сохранитьКакToolStripMenuItem.Click += сохранитьКакToolStripMenuItem_Click;
            // 
            // выходToolStripMenuItem
            // 
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            выходToolStripMenuItem.Size = new Size(251, 26);
            выходToolStripMenuItem.Text = "Выход";
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;
            // 
            // справкаToolStripMenuItem
            // 
            справкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { оПрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            справкаToolStripMenuItem.Size = new Size(81, 24);
            справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            оПрограммеToolStripMenuItem.Size = new Size(187, 26);
            оПрограммеToolStripMenuItem.Text = "О программе";
            оПрограммеToolStripMenuItem.Click += оПрограммеToolStripMenuItem_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(tabPage3);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 28);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1024, 636);
            tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(canvas);
            tabPage1.Controls.Add(task1ToolsPanel);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1016, 603);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Задание 1";
            // 
            // canvas
            // 
            canvas.BackColor = Color.White;
            canvas.BorderStyle = BorderStyle.FixedSingle;
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(223, 3);
            canvas.Name = "canvas";
            canvas.Size = new Size(790, 597);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.SizeChanged += canvas_SizeChanged;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
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
            task1ToolsPanel.Dock = DockStyle.Left;
            task1ToolsPanel.Location = new Point(3, 3);
            task1ToolsPanel.Name = "task1ToolsPanel";
            task1ToolsPanel.Size = new Size(220, 597);
            task1ToolsPanel.TabIndex = 1;
            // 
            // brushRadioButton
            // 
            brushRadioButton.AutoSize = true;
            brushRadioButton.Checked = true;
            brushRadioButton.Location = new Point(12, 14);
            brushRadioButton.Name = "brushRadioButton";
            brushRadioButton.Size = new Size(69, 24);
            brushRadioButton.TabIndex = 0;
            brushRadioButton.TabStop = true;
            brushRadioButton.Text = "Кисть";
            // 
            // lassoRadioButton
            // 
            lassoRadioButton.AutoSize = true;
            lassoRadioButton.Location = new Point(12, 70);
            lassoRadioButton.Name = "lassoRadioButton";
            lassoRadioButton.Size = new Size(237, 24);
            lassoRadioButton.TabIndex = 1;
            lassoRadioButton.Text = "Лассо (произвольная форма)";
            // 
            // eraserRadioButton
            // 
            eraserRadioButton.AutoSize = true;
            eraserRadioButton.Location = new Point(12, 42);
            eraserRadioButton.Name = "eraserRadioButton";
            eraserRadioButton.Size = new Size(77, 24);
            eraserRadioButton.TabIndex = 2;
            eraserRadioButton.Text = "Ластик";
            // 
            // contourColorButton
            // 
            contourColorButton.Location = new Point(12, 106);
            contourColorButton.Name = "contourColorButton";
            contourColorButton.Size = new Size(145, 28);
            contourColorButton.TabIndex = 3;
            contourColorButton.Text = "Цвет контура";
            contourColorButton.Click += contourColorButton_Click;
            // 
            // contourColorPanel
            // 
            contourColorPanel.BackColor = Color.Black;
            contourColorPanel.BorderStyle = BorderStyle.FixedSingle;
            contourColorPanel.Location = new Point(166, 110);
            contourColorPanel.Name = "contourColorPanel";
            contourColorPanel.Size = new Size(24, 20);
            contourColorPanel.TabIndex = 4;
            // 
            // fillColorButton
            // 
            fillColorButton.Location = new Point(12, 142);
            fillColorButton.Name = "fillColorButton";
            fillColorButton.Size = new Size(145, 28);
            fillColorButton.TabIndex = 5;
            fillColorButton.Text = "Цвет заливки";
            fillColorButton.Click += fillColorButton_Click;
            // 
            // fillColorPanel
            // 
            fillColorPanel.BackColor = Color.LightSkyBlue;
            fillColorPanel.BorderStyle = BorderStyle.FixedSingle;
            fillColorPanel.Location = new Point(166, 146);
            fillColorPanel.Name = "fillColorPanel";
            fillColorPanel.Size = new Size(24, 20);
            fillColorPanel.TabIndex = 6;
            // 
            // textureButton
            // 
            textureButton.Location = new Point(12, 178);
            textureButton.Name = "textureButton";
            textureButton.Size = new Size(178, 28);
            textureButton.TabIndex = 7;
            textureButton.Text = "Загрузить текстуру...";
            textureButton.Click += textureButton_Click;
            // 
            // texturePreview
            // 
            texturePreview.BorderStyle = BorderStyle.FixedSingle;
            texturePreview.Location = new Point(12, 214);
            texturePreview.Name = "texturePreview";
            texturePreview.Size = new Size(80, 80);
            texturePreview.SizeMode = PictureBoxSizeMode.Zoom;
            texturePreview.TabIndex = 8;
            texturePreview.TabStop = false;
            // 
            // traceBoundaryButton
            // 
            traceBoundaryButton.Location = new Point(12, 310);
            traceBoundaryButton.Name = "traceBoundaryButton";
            traceBoundaryButton.Size = new Size(178, 30);
            traceBoundaryButton.TabIndex = 9;
            traceBoundaryButton.Text = "Обойти границу";
            traceBoundaryButton.Click += traceBoundaryButton_Click;
            // 
            // undoButton
            // 
            undoButton.Location = new Point(12, 348);
            undoButton.Name = "undoButton";
            undoButton.Size = new Size(178, 30);
            undoButton.TabIndex = 10;
            undoButton.Text = "Отменить";
            undoButton.Click += undoButton_Click;
            // 
            // clearCanvasButton
            // 
            clearCanvasButton.Location = new Point(12, 386);
            clearCanvasButton.Name = "clearCanvasButton";
            clearCanvasButton.Size = new Size(178, 30);
            clearCanvasButton.TabIndex = 11;
            clearCanvasButton.Text = "Очистить";
            clearCanvasButton.Click += clearCanvasButton_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.Control;
            tabPage2.Controls.Add(lineCanvas);
            tabPage2.Controls.Add(task2ToolsPanel);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1016, 611);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Задание 2";
            // 
            // lineCanvas
            // 
            lineCanvas.BackColor = Color.White;
            lineCanvas.BorderStyle = BorderStyle.FixedSingle;
            lineCanvas.Dock = DockStyle.Fill;
            lineCanvas.Location = new Point(223, 3);
            lineCanvas.Name = "lineCanvas";
            lineCanvas.Size = new Size(790, 605);
            lineCanvas.TabIndex = 0;
            lineCanvas.TabStop = false;
            lineCanvas.SizeChanged += lineCanvas_SizeChanged;
            lineCanvas.MouseDown += lineCanvas_MouseDown;
            // 
            // task2ToolsPanel
            // 
            task2ToolsPanel.Controls.Add(bresenhamRadioButton);
            task2ToolsPanel.Controls.Add(wuRadioButton);
            task2ToolsPanel.Controls.Add(lineColorButton);
            task2ToolsPanel.Controls.Add(lineColorPanel);
            task2ToolsPanel.Controls.Add(lineThicknessLabel);
            task2ToolsPanel.Controls.Add(lineThicknessUpDown);
            task2ToolsPanel.Controls.Add(clearLineCanvasButton);
            task2ToolsPanel.Controls.Add(lineInfoLabel);
            task2ToolsPanel.Dock = DockStyle.Left;
            task2ToolsPanel.Location = new Point(3, 3);
            task2ToolsPanel.Name = "task2ToolsPanel";
            task2ToolsPanel.Size = new Size(220, 605);
            task2ToolsPanel.TabIndex = 1;
            // 
            // bresenhamRadioButton
            // 
            bresenhamRadioButton.AutoSize = true;
            bresenhamRadioButton.Checked = true;
            bresenhamRadioButton.Location = new Point(12, 14);
            bresenhamRadioButton.Name = "bresenhamRadioButton";
            bresenhamRadioButton.Size = new Size(232, 24);
            bresenhamRadioButton.TabIndex = 0;
            bresenhamRadioButton.TabStop = true;
            bresenhamRadioButton.Text = "Брезенхем (целочисленный)";
            // 
            // wuRadioButton
            // 
            wuRadioButton.AutoSize = true;
            wuRadioButton.Location = new Point(12, 42);
            wuRadioButton.Name = "wuRadioButton";
            wuRadioButton.Size = new Size(146, 24);
            wuRadioButton.TabIndex = 1;
            wuRadioButton.Text = "Ву (сглаженный)";
            // 
            // lineColorButton
            // 
            lineColorButton.Location = new Point(12, 80);
            lineColorButton.Name = "lineColorButton";
            lineColorButton.Size = new Size(145, 28);
            lineColorButton.TabIndex = 2;
            lineColorButton.Text = "Цвет линии";
            lineColorButton.Click += lineColorButton_Click;
            // 
            // lineColorPanel
            // 
            lineColorPanel.BackColor = Color.Black;
            lineColorPanel.BorderStyle = BorderStyle.FixedSingle;
            lineColorPanel.Location = new Point(166, 84);
            lineColorPanel.Name = "lineColorPanel";
            lineColorPanel.Size = new Size(24, 20);
            lineColorPanel.TabIndex = 3;
            // 
            // lineThicknessLabel
            // 
            lineThicknessLabel.AutoSize = true;
            lineThicknessLabel.Location = new Point(12, 122);
            lineThicknessLabel.Name = "lineThicknessLabel";
            lineThicknessLabel.Size = new Size(164, 20);
            lineThicknessLabel.TabIndex = 4;
            lineThicknessLabel.Text = "Толщина (для Ву — 1):";
            // 
            // lineThicknessUpDown
            // 
            lineThicknessUpDown.Location = new Point(12, 144);
            lineThicknessUpDown.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            lineThicknessUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            lineThicknessUpDown.Name = "lineThicknessUpDown";
            lineThicknessUpDown.Size = new Size(60, 27);
            lineThicknessUpDown.TabIndex = 5;
            lineThicknessUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // clearLineCanvasButton
            // 
            clearLineCanvasButton.Location = new Point(12, 185);
            clearLineCanvasButton.Name = "clearLineCanvasButton";
            clearLineCanvasButton.Size = new Size(178, 30);
            clearLineCanvasButton.TabIndex = 6;
            clearLineCanvasButton.Text = "Очистить";
            clearLineCanvasButton.Click += clearLineCanvasButton_Click;
            // 
            // lineInfoLabel
            // 
            lineInfoLabel.Location = new Point(12, 225);
            lineInfoLabel.Name = "lineInfoLabel";
            lineInfoLabel.Size = new Size(200, 200);
            lineInfoLabel.TabIndex = 7;
            lineInfoLabel.Text = "ЛКМ — задать начало отрезка.\nЛКМ — задать конец.";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = SystemColors.Control;
            tabPage3.Controls.Add(lblHintTask3);
            tabPage3.Controls.Add(btnClearTask3);
            tabPage3.Controls.Add(pictureBoxTask3);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1016, 603);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Задание 3";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 664);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1024, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(136, 20);
            statusLabel.Text = "Координаты: (0, 0)";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Изображения|*.bmp;*.jpg;*.jpeg;*.png;*.gif|Все файлы|*.*";
            openFileDialog.Title = "Открыть фон (текстуру)";
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "png";
            saveFileDialog.Filter = "PNG изображение|*.png|BMP изображение|*.bmp|JPEG изображение|*.jpg";
            saveFileDialog.Title = "Сохранить как";
            // 
            // pictureBoxTask3
            // 
            pictureBoxTask3.BackColor = Color.White;
            pictureBoxTask3.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxTask3.Location = new Point(30, 190);
            pictureBoxTask3.Name = "pictureBoxTask3";
            pictureBoxTask3.Size = new Size(951, 382);
            pictureBoxTask3.TabIndex = 0;
            pictureBoxTask3.TabStop = false;
            pictureBoxTask3.Click += pictureBoxTask3_Click;
            // 
            // btnClearTask3
            // 
            btnClearTask3.Location = new Point(830, 80);
            btnClearTask3.Name = "btnClearTask3";
            btnClearTask3.Size = new Size(151, 58);
            btnClearTask3.TabIndex = 1;
            btnClearTask3.Text = "Очистить";
            btnClearTask3.UseVisualStyleBackColor = true;
            btnClearTask3.Click += btnClearTask3_Click;
            // 
            // lblHintTask3
            // 
            lblHintTask3.AutoSize = true;
            lblHintTask3.ForeColor = Color.Gray;
            lblHintTask3.Location = new Point(30, 80);
            lblHintTask3.Name = "lblHintTask3";
            lblHintTask3.Size = new Size(393, 20);
            lblHintTask3.TabIndex = 2;
            lblHintTask3.Text = "Кликните 3 раза по полю — треугольник с градиентом";
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1024, 690);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Лабораторная работа №3 - Растровые алгоритмы";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            task1ToolsPanel.ResumeLayout(false);
            task1ToolsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)texturePreview).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)lineCanvas).EndInit();
            task2ToolsPanel.ResumeLayout(false);
            task2ToolsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lineThicknessUpDown).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTask3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}