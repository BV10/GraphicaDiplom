namespace WindowsFormsApplication2
{
    partial class FormSizeOfSquare
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
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxSizeOfSquare = new System.Windows.Forms.TextBox();
            this.butChooseSize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(83, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Укажите размер квадрата";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tBoxSizeOfSquare
            // 
            this.tBoxSizeOfSquare.Location = new System.Drawing.Point(141, 60);
            this.tBoxSizeOfSquare.Name = "tBoxSizeOfSquare";
            this.tBoxSizeOfSquare.Size = new System.Drawing.Size(78, 20);
            this.tBoxSizeOfSquare.TabIndex = 1;
            this.tBoxSizeOfSquare.Text = "80";
            this.tBoxSizeOfSquare.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tBoxSizeOfSquare.TextChanged += new System.EventHandler(this.tBoxSizeOfSquare_TextChanged);
            this.tBoxSizeOfSquare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxSizeOfSquare_KeyPress);
            // 
            // butChooseSize
            // 
            this.butChooseSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butChooseSize.Location = new System.Drawing.Point(131, 99);
            this.butChooseSize.Name = "butChooseSize";
            this.butChooseSize.Size = new System.Drawing.Size(100, 35);
            this.butChooseSize.TabIndex = 2;
            this.butChooseSize.Text = "OK";
            this.butChooseSize.UseVisualStyleBackColor = true;
            this.butChooseSize.Click += new System.EventHandler(this.butChooseSize_Click);
            // 
            // FormSizeOfSquare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 161);
            this.Controls.Add(this.butChooseSize);
            this.Controls.Add(this.tBoxSizeOfSquare);
            this.Controls.Add(this.label1);
            this.Name = "FormSizeOfSquare";
            this.Text = "FormSizeOfSquare";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxSizeOfSquare;
        private System.Windows.Forms.Button butChooseSize;
    }
}