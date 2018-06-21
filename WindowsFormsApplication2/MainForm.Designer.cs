using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.butLine = new System.Windows.Forms.Button();
            this.butCircle = new System.Windows.Forms.Button();
            this.butTriangle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.контактыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.разработчикToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.контактыToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.butColor = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.butClear = new System.Windows.Forms.Button();
            this.butBuildChoseFigure = new System.Windows.Forms.Button();
            this.butPencil = new System.Windows.Forms.Button();
            this.butSquare = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.butPouring = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxThicknessLine = new System.Windows.Forms.TextBox();
            this.butSaveImage = new System.Windows.Forms.Button();
            this.hScrollDotesPerSec = new System.Windows.Forms.HScrollBar();
            this.labelDotesPerSec = new System.Windows.Forms.Label();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.panelPaint = new System.Windows.Forms.Panel();
            this.инструкцияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.panelPaint.SuspendLayout();
            this.SuspendLayout();
            // 
            // butLine
            // 
            this.butLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butLine.Location = new System.Drawing.Point(10, 48);
            this.butLine.Name = "butLine";
            this.butLine.Size = new System.Drawing.Size(150, 26);
            this.butLine.TabIndex = 1;
            this.butLine.Text = "Линия";
            this.butLine.UseVisualStyleBackColor = true;
            this.butLine.Click += new System.EventHandler(this.butLine_Click);
            // 
            // butCircle
            // 
            this.butCircle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butCircle.Location = new System.Drawing.Point(11, 111);
            this.butCircle.Name = "butCircle";
            this.butCircle.Size = new System.Drawing.Size(150, 25);
            this.butCircle.TabIndex = 2;
            this.butCircle.Text = "Круг";
            this.butCircle.UseVisualStyleBackColor = true;
            this.butCircle.Click += new System.EventHandler(this.butCircle_Click);
            // 
            // butTriangle
            // 
            this.butTriangle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butTriangle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butTriangle.Location = new System.Drawing.Point(11, 80);
            this.butTriangle.Name = "butTriangle";
            this.butTriangle.Size = new System.Drawing.Size(150, 25);
            this.butTriangle.TabIndex = 3;
            this.butTriangle.Text = "Треугольник";
            this.butTriangle.UseVisualStyleBackColor = true;
            this.butTriangle.Click += new System.EventHandler(this.butTriangle_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Выберите фигуру";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.контактыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1277, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // контактыToolStripMenuItem
            // 
            this.контактыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.разработчикToolStripMenuItem,
            this.контактыToolStripMenuItem1,
            this.инструкцияToolStripMenuItem});
            this.контактыToolStripMenuItem.Name = "контактыToolStripMenuItem";
            this.контактыToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.контактыToolStripMenuItem.Text = "О Программе";
            // 
            // разработчикToolStripMenuItem
            // 
            this.разработчикToolStripMenuItem.Name = "разработчикToolStripMenuItem";
            this.разработчикToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.разработчикToolStripMenuItem.Text = "Разработчик";
            this.разработчикToolStripMenuItem.Click += new System.EventHandler(this.разработчикToolStripMenuItem_Click);
            // 
            // контактыToolStripMenuItem1
            // 
            this.контактыToolStripMenuItem1.Name = "контактыToolStripMenuItem1";
            this.контактыToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.контактыToolStripMenuItem1.Text = "Контакты";
            this.контактыToolStripMenuItem1.Click += new System.EventHandler(this.контактыToolStripMenuItem1_Click);
            // 
            // butColor
            // 
            this.butColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butColor.Location = new System.Drawing.Point(11, 420);
            this.butColor.Name = "butColor";
            this.butColor.Size = new System.Drawing.Size(156, 23);
            this.butColor.TabIndex = 15;
            this.butColor.Text = "Палитра цветов";
            this.butColor.UseVisualStyleBackColor = true;
            this.butColor.Click += new System.EventHandler(this.butColor_Click);
            // 
            // butClear
            // 
            this.butClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butClear.Location = new System.Drawing.Point(11, 475);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(156, 23);
            this.butClear.TabIndex = 16;
            this.butClear.Text = "Очистить";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // butBuildChoseFigure
            // 
            this.butBuildChoseFigure.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butBuildChoseFigure.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butBuildChoseFigure.Location = new System.Drawing.Point(11, 204);
            this.butBuildChoseFigure.Name = "butBuildChoseFigure";
            this.butBuildChoseFigure.Size = new System.Drawing.Size(150, 40);
            this.butBuildChoseFigure.TabIndex = 18;
            this.butBuildChoseFigure.Text = "Построить";
            this.butBuildChoseFigure.UseVisualStyleBackColor = true;
            this.butBuildChoseFigure.Click += new System.EventHandler(this.butBuildFigure_Click);
            // 
            // butPencil
            // 
            this.butPencil.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butPencil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butPencil.Location = new System.Drawing.Point(11, 173);
            this.butPencil.Name = "butPencil";
            this.butPencil.Size = new System.Drawing.Size(150, 25);
            this.butPencil.TabIndex = 19;
            this.butPencil.Text = "Карандаш";
            this.butPencil.UseVisualStyleBackColor = true;
            this.butPencil.Click += new System.EventHandler(this.butPencil_Click);
            // 
            // butSquare
            // 
            this.butSquare.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSquare.Location = new System.Drawing.Point(10, 142);
            this.butSquare.Name = "butSquare";
            this.butSquare.Size = new System.Drawing.Size(150, 25);
            this.butSquare.TabIndex = 20;
            this.butSquare.Text = "Квадрат";
            this.butSquare.UseVisualStyleBackColor = true;
            this.butSquare.Click += new System.EventHandler(this.butSquare_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 16);
            this.label2.TabIndex = 21;
            this.label2.Text = "Количество точек/сек";
            // 
            // butPouring
            // 
            this.butPouring.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butPouring.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butPouring.Location = new System.Drawing.Point(11, 392);
            this.butPouring.Name = "butPouring";
            this.butPouring.Size = new System.Drawing.Size(152, 23);
            this.butPouring.TabIndex = 23;
            this.butPouring.Text = "Заливка";
            this.butPouring.UseVisualStyleBackColor = true;
            this.butPouring.Click += new System.EventHandler(this.butPouring_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(24, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Толщина линии";
            // 
            // tBoxThicknessLine
            // 
            this.tBoxThicknessLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tBoxThicknessLine.Location = new System.Drawing.Point(52, 354);
            this.tBoxThicknessLine.Name = "tBoxThicknessLine";
            this.tBoxThicknessLine.Size = new System.Drawing.Size(45, 20);
            this.tBoxThicknessLine.TabIndex = 25;
            this.tBoxThicknessLine.Text = "4";
            this.tBoxThicknessLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tBoxThicknessLine.TextChanged += new System.EventHandler(this.tBoxThicknessLine_TextChanged);
            // 
            // butSaveImage
            // 
            this.butSaveImage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.butSaveImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butSaveImage.Location = new System.Drawing.Point(10, 447);
            this.butSaveImage.Name = "butSaveImage";
            this.butSaveImage.Size = new System.Drawing.Size(156, 23);
            this.butSaveImage.TabIndex = 26;
            this.butSaveImage.Text = "Сохранить";
            this.butSaveImage.UseVisualStyleBackColor = true;
            this.butSaveImage.Click += new System.EventHandler(this.button1_Click);
            // 
            // hScrollDotesPerSec
            // 
            this.hScrollDotesPerSec.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.hScrollDotesPerSec.Location = new System.Drawing.Point(35, 305);
            this.hScrollDotesPerSec.Maximum = 210;
            this.hScrollDotesPerSec.Minimum = 1;
            this.hScrollDotesPerSec.Name = "hScrollDotesPerSec";
            this.hScrollDotesPerSec.Size = new System.Drawing.Size(80, 16);
            this.hScrollDotesPerSec.TabIndex = 27;
            this.hScrollDotesPerSec.Value = 210;
            this.hScrollDotesPerSec.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // labelDotesPerSec
            // 
            this.labelDotesPerSec.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDotesPerSec.AutoSize = true;
            this.labelDotesPerSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDotesPerSec.Location = new System.Drawing.Point(44, 283);
            this.labelDotesPerSec.Name = "labelDotesPerSec";
            this.labelDotesPerSec.Size = new System.Drawing.Size(65, 16);
            this.labelDotesPerSec.TabIndex = 28;
            this.labelDotesPerSec.Text = "Maximum";
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(944, 477);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            this.pictureBoxImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseClick);
            this.pictureBoxImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseDown);
            this.pictureBoxImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseMove);
            this.pictureBoxImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseUp);
            this.pictureBoxImage.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseWheel);
            this.pictureBoxImage.Resize += new System.EventHandler(this.pictureBoxImage_Resize_1);
            // 
            // panelPaint
            // 
            this.panelPaint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPaint.AutoScroll = true;
            this.panelPaint.BackColor = System.Drawing.SystemColors.Control;
            this.panelPaint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPaint.Controls.Add(this.pictureBoxImage);
            this.panelPaint.Location = new System.Drawing.Point(174, 25);
            this.panelPaint.Name = "panelPaint";
            this.panelPaint.Size = new System.Drawing.Size(945, 479);
            this.panelPaint.TabIndex = 0;
            // 
            // инструкцияToolStripMenuItem
            // 
            this.инструкцияToolStripMenuItem.Name = "инструкцияToolStripMenuItem";
            this.инструкцияToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.инструкцияToolStripMenuItem.Text = "Инструкция";
            this.инструкцияToolStripMenuItem.Click += new System.EventHandler(this.инструкцияToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1277, 515);
            this.Controls.Add(this.labelDotesPerSec);
            this.Controls.Add(this.hScrollDotesPerSec);
            this.Controls.Add(this.butSaveImage);
            this.Controls.Add(this.tBoxThicknessLine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.butPouring);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.butSquare);
            this.Controls.Add(this.butPencil);
            this.Controls.Add(this.butBuildChoseFigure);
            this.Controls.Add(this.butClear);
            this.Controls.Add(this.butColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butTriangle);
            this.Controls.Add(this.butCircle);
            this.Controls.Add(this.butLine);
            this.Controls.Add(this.panelPaint);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Графический редактор";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.panelPaint.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion
        private System.Windows.Forms.Button butLine;
        private System.Windows.Forms.Button butCircle;
        private System.Windows.Forms.Button butTriangle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem контактыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem разработчикToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem контактыToolStripMenuItem1;
        private System.Windows.Forms.Button butColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button butClear;
        private System.Windows.Forms.Button butBuildChoseFigure;
        private System.Windows.Forms.Button butPencil;
        private System.Windows.Forms.Button butSquare;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.Button butPouring;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBoxThicknessLine;
        private System.Windows.Forms.Button butSaveImage;
        private System.Windows.Forms.HScrollBar hScrollDotesPerSec;
        private System.Windows.Forms.Label labelDotesPerSec;
        private PictureBox pictureBoxImage;
        private Panel panelPaint;
        private ToolStripMenuItem инструкцияToolStripMenuItem;
    }
}

