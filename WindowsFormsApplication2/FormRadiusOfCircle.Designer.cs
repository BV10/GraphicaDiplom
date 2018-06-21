namespace WindowsFormsApplication2
{
    partial class FormRadiusOfCircle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butChooseRadius = new System.Windows.Forms.Button();
            this.tBoxRadius = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // butChooseRadius
            // 
            this.butChooseRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butChooseRadius.Location = new System.Drawing.Point(138, 101);
            this.butChooseRadius.Name = "butChooseRadius";
            this.butChooseRadius.Size = new System.Drawing.Size(100, 35);
            this.butChooseRadius.TabIndex = 5;
            this.butChooseRadius.Text = "OK";
            this.butChooseRadius.UseVisualStyleBackColor = true;
            this.butChooseRadius.Click += new System.EventHandler(this.butChooseRadius_Click);
            // 
            // tBoxRadius
            // 
            this.tBoxRadius.Location = new System.Drawing.Point(148, 62);
            this.tBoxRadius.Name = "tBoxRadius";
            this.tBoxRadius.Size = new System.Drawing.Size(78, 20);
            this.tBoxRadius.TabIndex = 4;
            this.tBoxRadius.Text = "70";
            this.tBoxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tBoxRadius.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxRadius_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(74, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Укажите радиус окружности";
            // 
            // FormRadiusOfCircle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 161);
            this.Controls.Add(this.butChooseRadius);
            this.Controls.Add(this.tBoxRadius);
            this.Controls.Add(this.label1);
            this.Name = "FormRadiusOfCircle";
            this.Text = "Радиус окружности";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butChooseRadius;
        private System.Windows.Forms.TextBox tBoxRadius;
        private System.Windows.Forms.Label label1;
    }
}