namespace UniversidadXYZ.Presentacion.Procesos
{
    partial class FrmIngresNotas
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.lblTitulo          = new System.Windows.Forms.Label();
            this.lblDocente         = new System.Windows.Forms.Label();
            this.txtCodDocente      = new System.Windows.Forms.TextBox();
            this.lblGestion         = new System.Windows.Forms.Label();
            this.cmbGestion         = new System.Windows.Forms.ComboBox();
            this.lblEdicion         = new System.Windows.Forms.Label();
            this.cmbEdicion         = new System.Windows.Forms.ComboBox();
            this.lblGrupo           = new System.Windows.Forms.Label();
            this.cmbGrupo           = new System.Windows.Forms.ComboBox();
            this.lblEstadoEdicion   = new System.Windows.Forms.Label();
            this.btnCargar          = new System.Windows.Forms.Button();
            this.dgvNotas           = new System.Windows.Forms.DataGridView();
            this.btnModificar       = new System.Windows.Forms.Button();
            this.btnGrabar          = new System.Windows.Forms.Button();
            this.btnBloquear        = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvNotas)).BeginInit();
            this.SuspendLayout();

            // Título
            this.lblTitulo.Text      = "INGRESO DE NOTAS";
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location  = new System.Drawing.Point(10, 8);
            this.lblTitulo.Size      = new System.Drawing.Size(300, 28);

            // Fila 1
            this.lblDocente.Text     = "Cód. Docente:"; this.lblDocente.Location=new System.Drawing.Point(10,46); this.lblDocente.Size=new System.Drawing.Size(100,20); this.lblDocente.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtCodDocente.Location=new System.Drawing.Point(115,43); this.txtCodDocente.Size=new System.Drawing.Size(70,22); this.txtCodDocente.Name="txtCodDocente";
            this.txtCodDocente.Leave+=new System.EventHandler(this.txtCodDocente_Leave);

            this.lblGestion.Text     = "Gestión:"; this.lblGestion.Location=new System.Drawing.Point(200,46); this.lblGestion.Size=new System.Drawing.Size(70,20); this.lblGestion.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.cmbGestion.Location =new System.Drawing.Point(275,43); this.cmbGestion.Size=new System.Drawing.Size(110,22); this.cmbGestion.Name="cmbGestion"; this.cmbGestion.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGestion.SelectedIndexChanged+=new System.EventHandler(this.cmbGestion_SelectedIndexChanged);

            // Fila 2
            this.lblEdicion.Text     = "Edición/Mat.:"; this.lblEdicion.Location=new System.Drawing.Point(10,74); this.lblEdicion.Size=new System.Drawing.Size(100,20); this.lblEdicion.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.cmbEdicion.Location =new System.Drawing.Point(115,71); this.cmbEdicion.Size=new System.Drawing.Size(180,22); this.cmbEdicion.Name="cmbEdicion"; this.cmbEdicion.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdicion.SelectedIndexChanged+=new System.EventHandler(this.cmbEdicion_SelectedIndexChanged);

            this.lblGrupo.Text       = "Grupo:"; this.lblGrupo.Location=new System.Drawing.Point(305,74); this.lblGrupo.Size=new System.Drawing.Size(60,20); this.lblGrupo.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.cmbGrupo.Location   =new System.Drawing.Point(370,71); this.cmbGrupo.Size=new System.Drawing.Size(100,22); this.cmbGrupo.Name="cmbGrupo"; this.cmbGrupo.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.btnCargar.Text      = "Cargar Estudiantes";
            this.btnCargar.Location  = new System.Drawing.Point(485,69);
            this.btnCargar.Size      = new System.Drawing.Size(150,28);
            this.btnCargar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCargar.ForeColor = System.Drawing.Color.White;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.Click    += new System.EventHandler(this.btnCargar_Click);

            // Estado edición
            this.lblEstadoEdicion.Text      = "";
            this.lblEstadoEdicion.Location  = new System.Drawing.Point(10, 102);
            this.lblEstadoEdicion.Size      = new System.Drawing.Size(400, 22);
            this.lblEstadoEdicion.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // DataGridView notas
            this.dgvNotas.Location = new System.Drawing.Point(10, 128);
            this.dgvNotas.Size     = new System.Drawing.Size(760, 330);
            this.dgvNotas.Name     = "dgvNotas";
            this.dgvNotas.ReadOnly = true;
            this.dgvNotas.AllowUserToAddRows = false;
            this.dgvNotas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNotas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNotas.MultiSelect = false;
            this.dgvNotas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;

            // Botones acción
            int bx = 10, by = 470, bw = 120, bh = 34;
            this.btnModificar.Text = "✏ Modificar"; this.btnModificar.Location=new System.Drawing.Point(bx,by); this.btnModificar.Size=new System.Drawing.Size(bw,bh);
            this.btnModificar.BackColor=System.Drawing.Color.DarkOrange; this.btnModificar.ForeColor=System.Drawing.Color.White; this.btnModificar.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar.Font=new System.Drawing.Font("Segoe UI",9F,System.Drawing.FontStyle.Bold);
            this.btnModificar.Click+=new System.EventHandler(this.btnModificar_Click);

            this.btnGrabar.Text="💾 Grabar"; this.btnGrabar.Location=new System.Drawing.Point(bx+bw+10,by); this.btnGrabar.Size=new System.Drawing.Size(bw,bh);
            this.btnGrabar.BackColor=System.Drawing.Color.SeaGreen; this.btnGrabar.ForeColor=System.Drawing.Color.White; this.btnGrabar.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
            this.btnGrabar.Font=new System.Drawing.Font("Segoe UI",9F,System.Drawing.FontStyle.Bold);
            this.btnGrabar.Enabled=false;
            this.btnGrabar.Click+=new System.EventHandler(this.btnGrabar_Click);

            this.btnBloquear.Text="🔒 Bloquear"; this.btnBloquear.Location=new System.Drawing.Point(bx+2*(bw+10),by); this.btnBloquear.Size=new System.Drawing.Size(bw,bh);
            this.btnBloquear.BackColor=System.Drawing.Color.Crimson; this.btnBloquear.ForeColor=System.Drawing.Color.White; this.btnBloquear.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
            this.btnBloquear.Font=new System.Drawing.Font("Segoe UI",9F,System.Drawing.FontStyle.Bold);
            this.btnBloquear.Click+=new System.EventHandler(this.btnBloquear_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(790, 515);
            this.Text                = "Ingreso de Notas";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{
                lblTitulo,
                lblDocente,txtCodDocente,lblGestion,cmbGestion,
                lblEdicion,cmbEdicion,lblGrupo,cmbGrupo,btnCargar,
                lblEstadoEdicion,
                dgvNotas,
                btnModificar,btnGrabar,btnBloquear
            });
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotas)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label      lblTitulo, lblDocente, lblGestion, lblEdicion, lblGrupo, lblEstadoEdicion;
        private System.Windows.Forms.TextBox    txtCodDocente;
        private System.Windows.Forms.ComboBox   cmbGestion, cmbEdicion, cmbGrupo;
        private System.Windows.Forms.Button     btnCargar, btnModificar, btnGrabar, btnBloquear;
        private System.Windows.Forms.DataGridView dgvNotas;
    }
}
