using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnCrearArchivo = new Button();
            BtnMoverArchivo = new Button();
            labelDatos = new Label();
            btnEliminar = new Button();
            btnCopiar = new Button();
            btnVerPropiedades = new Button();
            dgvDatos = new DataGridView();
            dgvPropiedades = new DataGridView();
            btnAbrir = new Button();
            btnModificar = new Button();
            btnCrearCarpeta = new Button();
            btnRenombrar = new Button();
            btnAgregarInfo = new Button();
            btnEliminarInfo = new Button();
            btnBuscar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDatos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPropiedades).BeginInit();
            SuspendLayout();
            // 
            // BtnCrearArchivo
            // 
            BtnCrearArchivo.Location = new Point(36, 388);
            BtnCrearArchivo.Margin = new Padding(3, 2, 3, 2);
            BtnCrearArchivo.Name = "BtnCrearArchivo";
            BtnCrearArchivo.Size = new Size(158, 46);
            BtnCrearArchivo.TabIndex = 2;
            BtnCrearArchivo.Text = "Crear Archivo";
            BtnCrearArchivo.UseVisualStyleBackColor = true;
            BtnCrearArchivo.Click += BtnCrearArchivo_click;
            // 
            // BtnMoverArchivo
            // 
            BtnMoverArchivo.Location = new Point(388, 388);
            BtnMoverArchivo.Margin = new Padding(3, 2, 3, 2);
            BtnMoverArchivo.Name = "BtnMoverArchivo";
            BtnMoverArchivo.Size = new Size(156, 46);
            BtnMoverArchivo.TabIndex = 3;
            BtnMoverArchivo.Text = "Mover Archivo";
            BtnMoverArchivo.UseVisualStyleBackColor = true;
            BtnMoverArchivo.Click += BtnMoverArchivo_Click;
            // 
            // labelDatos
            // 
            labelDatos.AutoSize = true;
            labelDatos.Location = new Point(10, 9);
            labelDatos.Name = "labelDatos";
            labelDatos.Size = new Size(97, 15);
            labelDatos.TabIndex = 0;
            labelDatos.Text = "Escriba los datos:";
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(206, 388);
            btnEliminar.Margin = new Padding(3, 2, 3, 2);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(153, 46);
            btnEliminar.TabIndex = 4;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnCopiar
            // 
            btnCopiar.Location = new Point(573, 388);
            btnCopiar.Margin = new Padding(3, 2, 3, 2);
            btnCopiar.Name = "btnCopiar";
            btnCopiar.Size = new Size(148, 46);
            btnCopiar.TabIndex = 5;
            btnCopiar.Text = "Copiar";
            btnCopiar.UseVisualStyleBackColor = true;
            btnCopiar.Click += btnCopiar_Click;
            // 
            // btnVerPropiedades
            // 
            btnVerPropiedades.Location = new Point(954, 388);
            btnVerPropiedades.Margin = new Padding(3, 2, 3, 2);
            btnVerPropiedades.Name = "btnVerPropiedades";
            btnVerPropiedades.Size = new Size(184, 46);
            btnVerPropiedades.TabIndex = 6;
            btnVerPropiedades.Text = "Ver propiedades";
            btnVerPropiedades.UseVisualStyleBackColor = true;
            btnVerPropiedades.Click += btnVerPropiedades_Click;
            // 
            // dgvDatos
            // 
            dgvDatos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDatos.Location = new Point(10, 26);
            dgvDatos.Margin = new Padding(3, 2, 3, 2);
            dgvDatos.Name = "dgvDatos";
            dgvDatos.RowHeadersWidth = 51;
            dgvDatos.Size = new Size(760, 285);
            dgvDatos.TabIndex = 8;
            // 
            // dgvPropiedades
            // 
            dgvPropiedades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPropiedades.Location = new Point(814, 26);
            dgvPropiedades.Margin = new Padding(3, 2, 3, 2);
            dgvPropiedades.Name = "dgvPropiedades";
            dgvPropiedades.RowHeadersWidth = 51;
            dgvPropiedades.Size = new Size(541, 285);
            dgvPropiedades.TabIndex = 9;
            // 
            // btnAbrir
            // 
            btnAbrir.Location = new Point(36, 458);
            btnAbrir.Margin = new Padding(3, 2, 3, 2);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(158, 46);
            btnAbrir.TabIndex = 10;
            btnAbrir.Text = "Abrir Archivo";
            btnAbrir.UseVisualStyleBackColor = true;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(754, 388);
            btnModificar.Margin = new Padding(3, 2, 3, 2);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(175, 46);
            btnModificar.TabIndex = 11;
            btnModificar.Text = "Modificar Archivo";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnCrearCarpeta
            // 
            btnCrearCarpeta.Location = new Point(206, 458);
            btnCrearCarpeta.Margin = new Padding(3, 2, 3, 2);
            btnCrearCarpeta.Name = "btnCrearCarpeta";
            btnCrearCarpeta.Size = new Size(153, 46);
            btnCrearCarpeta.TabIndex = 12;
            btnCrearCarpeta.Text = "Crear Carpeta";
            btnCrearCarpeta.UseVisualStyleBackColor = true;
            btnCrearCarpeta.Click += btnCrearCarpeta_Click;
            // 
            // btnRenombrar
            // 
            btnRenombrar.Location = new Point(388, 458);
            btnRenombrar.Margin = new Padding(3, 2, 3, 2);
            btnRenombrar.Name = "btnRenombrar";
            btnRenombrar.Size = new Size(156, 46);
            btnRenombrar.TabIndex = 13;
            btnRenombrar.Text = "Renombrar";
            btnRenombrar.UseVisualStyleBackColor = true;
            btnRenombrar.Click += btnRenombrar_Click;
            // 
            // btnAgregarInfo
            // 
            btnAgregarInfo.Location = new Point(573, 458);
            btnAgregarInfo.Margin = new Padding(3, 2, 3, 2);
            btnAgregarInfo.Name = "btnAgregarInfo";
            btnAgregarInfo.Size = new Size(148, 46);
            btnAgregarInfo.TabIndex = 14;
            btnAgregarInfo.Text = "Agregar Info";
            btnAgregarInfo.UseVisualStyleBackColor = true;
            btnAgregarInfo.Click += btnAgregarInfo_Click;
            // 
            // btnEliminarInfo
            // 
            btnEliminarInfo.Location = new Point(754, 458);
            btnEliminarInfo.Margin = new Padding(3, 2, 3, 2);
            btnEliminarInfo.Name = "btnEliminarInfo";
            btnEliminarInfo.Size = new Size(175, 46);
            btnEliminarInfo.TabIndex = 15;
            btnEliminarInfo.Text = "Eliminar Info";
            btnEliminarInfo.UseVisualStyleBackColor = true;
            btnEliminarInfo.Click += btnEliminarInfo_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(954, 458);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(184, 46);
            btnBuscar.TabIndex = 16;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1370, 543);
            Controls.Add(btnBuscar);
            Controls.Add(btnEliminarInfo);
            Controls.Add(btnAgregarInfo);
            Controls.Add(btnRenombrar);
            Controls.Add(btnCrearCarpeta);
            Controls.Add(btnModificar);
            Controls.Add(btnAbrir);
            Controls.Add(dgvPropiedades);
            Controls.Add(dgvDatos);
            Controls.Add(btnVerPropiedades);
            Controls.Add(btnCopiar);
            Controls.Add(btnEliminar);
            Controls.Add(BtnMoverArchivo);
            Controls.Add(BtnCrearArchivo);
            Controls.Add(labelDatos);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Archivos Secuenciales";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDatos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPropiedades).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnCrearArchivo;
        private Button BtnMoverArchivo;
        private Label labelDatos;
        private Button btnEliminar;
        private Button btnCopiar;
        private Button btnVerPropiedades;
        private DataGridView dgvDatos;
        private DataGridView dgvPropiedades;
        private Button btnAbrir;
        private Button btnModificar;
        private Button btnCrearCarpeta;
        private Button btnRenombrar;
        private Button btnAgregarInfo;
        private Button btnEliminarInfo;
        private Button btnBuscar;
    }
}
