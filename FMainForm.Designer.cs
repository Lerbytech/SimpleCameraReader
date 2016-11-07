namespace SimpleCameraReader
{
  partial class FMainForm
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
      this.PictureBox = new System.Windows.Forms.PictureBox();
      this.StartButton = new System.Windows.Forms.Button();
      this.label_scanned_images = new System.Windows.Forms.Label();
      this.label_queue_images = new System.Windows.Forms.Label();
      this.BTN_Kill = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.BTN_SetExposition = new System.Windows.Forms.Button();
      this.TB_Gain = new System.Windows.Forms.TextBox();
      this.Combo_ImageFormats = new System.Windows.Forms.ComboBox();
      this.Combo_PixelFormats = new System.Windows.Forms.ComboBox();
      this.TB_Exposition = new System.Windows.Forms.TextBox();
      this.BTN_SetGain = new System.Windows.Forms.Button();
      this.PauseBtn = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label_saved_images = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // PictureBox
      // 
      this.PictureBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.PictureBox.Location = new System.Drawing.Point(3, 3);
      this.PictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
      this.PictureBox.Name = "PictureBox";
      this.PictureBox.Size = new System.Drawing.Size(1272, 500);
      this.PictureBox.TabIndex = 0;
      this.PictureBox.TabStop = false;
      // 
      // StartButton
      // 
      this.StartButton.CausesValidation = false;
      this.StartButton.Location = new System.Drawing.Point(13, 5);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(59, 23);
      this.StartButton.TabIndex = 1;
      this.StartButton.Text = "Start";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // label_scanned_images
      // 
      this.label_scanned_images.AutoSize = true;
      this.label_scanned_images.Location = new System.Drawing.Point(143, 40);
      this.label_scanned_images.Name = "label_scanned_images";
      this.label_scanned_images.Size = new System.Drawing.Size(13, 13);
      this.label_scanned_images.TabIndex = 3;
      this.label_scanned_images.Text = "0";
      // 
      // label_queue_images
      // 
      this.label_queue_images.AutoSize = true;
      this.label_queue_images.Location = new System.Drawing.Point(143, 66);
      this.label_queue_images.Name = "label_queue_images";
      this.label_queue_images.Size = new System.Drawing.Size(13, 13);
      this.label_queue_images.TabIndex = 4;
      this.label_queue_images.Text = "0";
      // 
      // BTN_Kill
      // 
      this.BTN_Kill.Location = new System.Drawing.Point(143, 5);
      this.BTN_Kill.Name = "BTN_Kill";
      this.BTN_Kill.Size = new System.Drawing.Size(75, 23);
      this.BTN_Kill.TabIndex = 5;
      this.BTN_Kill.Text = "Kill";
      this.BTN_Kill.UseVisualStyleBackColor = true;
      this.BTN_Kill.Click += new System.EventHandler(this.BTN_Kill_Click);
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add(this.PictureBox);
      this.panel1.Location = new System.Drawing.Point(13, 82);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1285, 506);
      this.panel1.TabIndex = 6;
      // 
      // BTN_SetExposition
      // 
      this.BTN_SetExposition.Location = new System.Drawing.Point(421, 1);
      this.BTN_SetExposition.Name = "BTN_SetExposition";
      this.BTN_SetExposition.Size = new System.Drawing.Size(31, 23);
      this.BTN_SetExposition.TabIndex = 7;
      this.BTN_SetExposition.Text = "OK";
      this.BTN_SetExposition.UseVisualStyleBackColor = true;
      this.BTN_SetExposition.Click += new System.EventHandler(this.BTN_SetExposition_Click);
      // 
      // TB_Gain
      // 
      this.TB_Gain.Location = new System.Drawing.Point(530, 4);
      this.TB_Gain.Name = "TB_Gain";
      this.TB_Gain.Size = new System.Drawing.Size(100, 20);
      this.TB_Gain.TabIndex = 8;
      // 
      // Combo_ImageFormats
      // 
      this.Combo_ImageFormats.FormattingEnabled = true;
      this.Combo_ImageFormats.Location = new System.Drawing.Point(764, 6);
      this.Combo_ImageFormats.Name = "Combo_ImageFormats";
      this.Combo_ImageFormats.Size = new System.Drawing.Size(235, 21);
      this.Combo_ImageFormats.TabIndex = 9;
      this.Combo_ImageFormats.SelectedIndexChanged += new System.EventHandler(this.Combo_ImageFormats_SelectedIndexChanged);
      // 
      // Combo_PixelFormats
      // 
      this.Combo_PixelFormats.FormattingEnabled = true;
      this.Combo_PixelFormats.Location = new System.Drawing.Point(1005, 7);
      this.Combo_PixelFormats.Name = "Combo_PixelFormats";
      this.Combo_PixelFormats.Size = new System.Drawing.Size(235, 21);
      this.Combo_PixelFormats.TabIndex = 10;
      this.Combo_PixelFormats.SelectedIndexChanged += new System.EventHandler(this.Combo_PixelFormats_SelectedIndexChanged);
      // 
      // TB_Exposition
      // 
      this.TB_Exposition.Location = new System.Drawing.Point(315, 4);
      this.TB_Exposition.Name = "TB_Exposition";
      this.TB_Exposition.Size = new System.Drawing.Size(100, 20);
      this.TB_Exposition.TabIndex = 12;
      // 
      // BTN_SetGain
      // 
      this.BTN_SetGain.Location = new System.Drawing.Point(636, 3);
      this.BTN_SetGain.Name = "BTN_SetGain";
      this.BTN_SetGain.Size = new System.Drawing.Size(31, 23);
      this.BTN_SetGain.TabIndex = 11;
      this.BTN_SetGain.Text = "OK";
      this.BTN_SetGain.UseVisualStyleBackColor = true;
      this.BTN_SetGain.Click += new System.EventHandler(this.BTN_SetGain_Click);
      // 
      // PauseBtn
      // 
      this.PauseBtn.CausesValidation = false;
      this.PauseBtn.Location = new System.Drawing.Point(78, 5);
      this.PauseBtn.Name = "PauseBtn";
      this.PauseBtn.Size = new System.Drawing.Size(59, 23);
      this.PauseBtn.TabIndex = 1;
      this.PauseBtn.Text = "Pause";
      this.PauseBtn.UseVisualStyleBackColor = true;
      this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(79, 40);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(58, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Получено:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(238, 6);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(71, 13);
      this.label4.TabIndex = 15;
      this.label4.Text = "Экспозиция:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(464, 7);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(60, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Усиление:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(52, 66);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(87, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Длина очереди:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(73, 53);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(64, 13);
      this.label6.TabIndex = 18;
      this.label6.Text = "Сохранено:";
      // 
      // label_saved_images
      // 
      this.label_saved_images.AutoSize = true;
      this.label_saved_images.Location = new System.Drawing.Point(143, 53);
      this.label_saved_images.Name = "label_saved_images";
      this.label_saved_images.Size = new System.Drawing.Size(13, 13);
      this.label_saved_images.TabIndex = 19;
      this.label_saved_images.Text = "0";
      // 
      // FMainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1310, 600);
      this.Controls.Add(this.label_saved_images);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.TB_Exposition);
      this.Controls.Add(this.BTN_SetGain);
      this.Controls.Add(this.Combo_PixelFormats);
      this.Controls.Add(this.Combo_ImageFormats);
      this.Controls.Add(this.TB_Gain);
      this.Controls.Add(this.BTN_SetExposition);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.BTN_Kill);
      this.Controls.Add(this.label_queue_images);
      this.Controls.Add(this.label_scanned_images);
      this.Controls.Add(this.PauseBtn);
      this.Controls.Add(this.StartButton);
      this.Name = "FMainForm";
      this.Text = "Simple Camera Reader";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMainForm_FormClosing);
      this.Load += new System.EventHandler(this.FMainForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox PictureBox;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.Label label_scanned_images;
    private System.Windows.Forms.Label label_queue_images;
    private System.Windows.Forms.Button BTN_Kill;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button BTN_SetExposition;
    private System.Windows.Forms.TextBox TB_Gain;
    private System.Windows.Forms.ComboBox Combo_ImageFormats;
    private System.Windows.Forms.ComboBox Combo_PixelFormats;
    private System.Windows.Forms.TextBox TB_Exposition;
    private System.Windows.Forms.Button BTN_SetGain;
    private System.Windows.Forms.Button PauseBtn;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label_saved_images;
  }
}

