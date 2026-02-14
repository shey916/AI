using System.IO;

namespace AI
{
    public partial class Form1 : Form
    {
        private string archivoActual = "";
        private const string FILTRO_ARCHIVOS = "Archivos de datos (*.dat)|*.dat|Todos los archivos (*.*)|*.*";
        public struct Indice
        {
            public int Id;
            public long Posicion;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridViews();
        }

        private void ConfigurarDataGridViews()
        {
            // Configuración para el archivo Indexado (ID + Contenido)
            dgvDatos.Columns.Clear();
            dgvDatos.Columns.Add("ID", "ID (Índice)");
            dgvDatos.Columns.Add("Datos", "Información");
            dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configuración para Propiedades (Evita el error que te salió)
            dgvPropiedades.Columns.Clear();
            dgvPropiedades.Columns.Add("Propiedad", "Propiedad");
            dgvPropiedades.Columns.Add("Valor", "Valor");
            dgvPropiedades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CrearArchivo()
        {
            try
            {
                if (dgvDatos.Rows.Count == 0 || string.IsNullOrWhiteSpace(dgvDatos.Rows[0].Cells[0].Value?.ToString()))
                {
                    MessageBox.Show("Por favor, escriba algo antes de crear el archivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = FILTRO_ARCHIVOS;
                saveFileDialog1.Title = "Guardar archivo";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = saveFileDialog1.FileName;
                    string extension = Path.GetExtension(rutaArchivo).ToLower();

                    if (File.Exists(rutaArchivo))
                    {
                        DialogResult resultado = MessageBox.Show("El archivo ya existe. �Desea reemplazarlo?", "Confirmaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resultado == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // Determinar el formato seg�n la extensi�n
                    switch (extension)
                    {
                        case ".json":
                            CrearArchivoJSON(rutaArchivo);
                            break;
                        case ".csv":
                            CrearArchivoCSV(rutaArchivo);
                            break;
                        case ".txt":
                        case ".dat":
                        default:
                            CrearArchivoTexto(rutaArchivo);
                            break;
                    }

                    MessageBox.Show($"Archivo {extension.ToUpper()} creado exitosamente.", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    archivoActual = rutaArchivo;
                    dgvDatos.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CrearArchivoTexto(string rutaArchivo)
        {
            string rutaIndice = Path.ChangeExtension(rutaArchivo, ".idx");

            using (BinaryWriter writerDatos = new BinaryWriter(File.Open(rutaArchivo, FileMode.Create)))
            using (BinaryWriter writerIndice = new BinaryWriter(File.Open(rutaIndice, FileMode.Create)))
            {
                foreach (DataGridViewRow row in dgvDatos.Rows)
                {
                    if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        // 1. Obtener ID y Posición
                        int id = int.Parse(row.Cells[0].Value.ToString());
                        long posicion = writerDatos.BaseStream.Position;

                        // 2. Escribir en el archivo de Índice
                        writerIndice.Write(id);
                        writerIndice.Write(posicion);

                        // 3. Escribir en el archivo de Datos
                        writerDatos.Write(row.Cells[1].Value.ToString());
                    }
                }
            }
        }

        private void CrearArchivoCSV(string rutaArchivo)
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivo))
            {
                foreach (DataGridViewRow row in dgvDatos.Rows)
                {
                    if (!row.IsNewRow && row.Cells[0].Value != null)
                    {
                        string valor = row.Cells[0].Value.ToString();

                        // Si el valor contiene comas, comillas o saltos de l�nea, lo encerramos entre comillas
                        if (valor.Contains(',') || valor.Contains('"') || valor.Contains('\n'))
                        {
                            valor = $"\"{valor.Replace("\"", "\"\"")}\"";
                        }

                        writer.WriteLine(valor);
                    }
                }
            }
        }

        private void CrearArchivoJSON(string rutaArchivo)
        {
            List<string> datos = new List<string>();

            foreach (DataGridViewRow row in dgvDatos.Rows)
            {
                if (!row.IsNewRow && row.Cells[0].Value != null)
                {
                    datos.Add(row.Cells[0].Value.ToString());
                }
            }

            // Serializar a JSON con formato indentado
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(datos, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(rutaArchivo, jsonContent);
        }

        private void MoverArchivo()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = FILTRO_ARCHIVOS;
                openFileDialog1.Title = "Seleccionar archivo a mover";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaOrigen = openFileDialog1.FileName;

                    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                    folderBrowserDialog1.Description = "Seleccione la carpeta de destino";
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string carpetaDestino = folderBrowserDialog1.SelectedPath;
                        string nombreArchivo = Path.GetFileName(rutaOrigen);
                        string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

                        if (File.Exists(rutaDestino))
                        {
                            DialogResult resultado = MessageBox.Show("El archivo ya existe en la carpeta de destino. �Desea reemplazarlo?", "Confirmaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (resultado == DialogResult.No)
                            {
                                return;
                            }
                            File.Delete(rutaDestino);
                        }

                        File.Move(rutaOrigen, rutaDestino);
                        MessageBox.Show("Archivo movido exitosamente.", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mover el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCrearArchivo_click(object sender, EventArgs e)
        {
            CrearArchivo();
        }

        private void BtnMoverArchivo_Click(object sender, EventArgs e)
        {
            MoverArchivo();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            bool flowControl = Eliminar();
            if (!flowControl)
            {
                return;
            }
        }

        private static bool Eliminar()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de texto (*.txt)|*.txt|Archivos CSV (*.csv)|*.csv|Archivos JSON (*.json)|*.json|Archivos de datos (*.dat)|*.dat|Todos los archivos (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                DialogResult resultado = MessageBox.Show("�Desea eliminarlo?", "Confirmaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.No)
                {
                    return false;
                }

                File.Delete(filepath);
                MessageBox.Show("Archivo eliminado exitosamente.", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return true;
        }

        private void CopiarArchivo()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = FILTRO_ARCHIVOS;
                openFileDialog1.Title = "Seleccionar archivo a copiar";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaOrigen = openFileDialog1.FileName;

                    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                    folderBrowserDialog1.Description = "Seleccione la carpeta de destino";
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string carpetaDestino = folderBrowserDialog1.SelectedPath;
                        string nombreArchivo = Path.GetFileName(rutaOrigen);
                        string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

                        if (File.Exists(rutaDestino))
                        {
                            DialogResult resultado = MessageBox.Show("El archivo ya existe en la carpeta de destino. �Desea reemplazarlo?", "Confirmaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (resultado == DialogResult.No)
                            {
                                return;
                            }
                            File.Delete(rutaDestino);
                        }

                        File.Copy(rutaOrigen, rutaDestino);
                        MessageBox.Show("Archivo copiado exitosamente.", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al copiar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            CopiarArchivo();
        }

        private void btnVerPropiedades_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = FILTRO_ARCHIVOS;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string rutaOrigen = openFileDialog1.FileName;
                FileInfo info = new FileInfo(rutaOrigen);

                // Asegurar que existan las columnas para evitar el error de la imagen
                if (dgvPropiedades.Columns.Count == 0)
                {
                    dgvPropiedades.Columns.Add("Propiedad", "Propiedad");
                    dgvPropiedades.Columns.Add("Valor", "Valor");
                }

                dgvPropiedades.Rows.Clear();
                dgvPropiedades.Rows.Add("Tamaño", info.Length + " bytes");
                dgvPropiedades.Rows.Add("Nombre", info.Name);
                dgvPropiedades.Rows.Add("Extensión", info.Extension);
                // ... resto de las propiedades
            }
        }

        private void AbrirArchivo()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = FILTRO_ARCHIVOS;
                openFileDialog1.Title = "Abrir archivo";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = openFileDialog1.FileName;
                    string extension = Path.GetExtension(rutaArchivo).ToLower();
                    archivoActual = rutaArchivo;

                    // limpiar el DataGridView antes de cargar
                    dgvDatos.Rows.Clear();

                    // Leer seg�n el tipo de archivo
                    switch (extension)
                    {
                        case ".json":
                            LeerArchivoJSON(rutaArchivo);
                            break;
                        case ".csv":
                            LeerArchivoCSV(rutaArchivo);
                            break;
                        case ".txt":
                        case ".dat":
                        default:
                            LeerArchivoTexto(rutaArchivo);
                            break;
                    }

                    MessageBox.Show($"Archivo {extension.ToUpper()} abierto exitosamente.\nL�neas le�das: {dgvDatos.Rows.Count - 1}",
                                    "�xito",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    this.Text = $"Archivos Secuenciales - {Path.GetFileName(rutaArchivo)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el archivo: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void LeerArchivoTexto(string rutaArchivo)
        {
            string rutaIndice = Path.ChangeExtension(rutaArchivo, ".idx");
            if (!File.Exists(rutaIndice)) return;

            dgvDatos.Rows.Clear();
            using (BinaryReader readerIndice = new BinaryReader(File.Open(rutaIndice, FileMode.Open)))
            using (BinaryReader readerDatos = new BinaryReader(File.Open(rutaArchivo, FileMode.Open)))
            {
                while (readerIndice.BaseStream.Position < readerIndice.BaseStream.Length)
                {
                    int id = readerIndice.ReadInt32();
                    long posicion = readerIndice.ReadInt64();

                    // Acceso indexado: Saltamos al lugar exacto
                    readerDatos.BaseStream.Seek(posicion, SeekOrigin.Begin);
                    string contenido = readerDatos.ReadString();

                    dgvDatos.Rows.Add(id, contenido);
                }
            }
        }

        private void LeerArchivoCSV(string rutaArchivo)
        {
            using (StreamReader reader = new StreamReader(rutaArchivo))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    // Remover comillas si el valor est� encerrado
                    if (linea.StartsWith("\"") && linea.EndsWith("\""))
                    {
                        linea = linea.Substring(1, linea.Length - 2).Replace("\"\"", "\"");
                    }
                    dgvDatos.Rows.Add(linea);
                }
            }
        }

        private void LeerArchivoJSON(string rutaArchivo)
        {
            string contenido = File.ReadAllText(rutaArchivo);

            try
            {
                // Intentar deserializar como array de strings
                var datos = System.Text.Json.JsonSerializer.Deserialize<List<string>>(contenido);

                if (datos != null)
                {
                    foreach (string item in datos)
                    {
                        dgvDatos.Rows.Add(item);
                    }
                }
            }
            catch
            {
                // Si falla, mostrar el contenido JSON como texto
                dgvDatos.Rows.Add(contenido);
            }
        }

        private void ModificarArchivo()
        {
            try
            {
                // si no hay archivo abierto, preguntar cu�l modificar
                if (string.IsNullOrEmpty(archivoActual))
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = FILTRO_ARCHIVOS;
                    openFileDialog1.Title = "Seleccionar archivo a modificar";

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        archivoActual = openFileDialog1.FileName;
                        string extension = Path.GetExtension(archivoActual).ToLower();

                        // cargar el contenido actual
                        dgvDatos.Rows.Clear();

                        switch (extension)
                        {
                            case ".json":
                                LeerArchivoJSON(archivoActual);
                                break;
                            case ".csv":
                                LeerArchivoCSV(archivoActual);
                                break;
                            default:
                                LeerArchivoTexto(archivoActual);
                                break;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                // verificar que hay datos para guardar
                if (dgvDatos.Rows.Count == 0 ||
                    (dgvDatos.Rows.Count == 1 && dgvDatos.Rows[0].IsNewRow))
                {
                    MessageBox.Show("No hay datos para guardar.",
                                   "Advertencia",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    return;
                }

                // Confirmar modificaci�n
                DialogResult resultado = MessageBox.Show(
                    $"�Desea guardar los cambios en el archivo?\n{Path.GetFileName(archivoActual)}",
                    "Confirmar modificaci�n",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    string extension = Path.GetExtension(archivoActual).ToLower();

                    // crear un archivo temporal para escritura segura
                    string archivoTemporal = archivoActual + ".tmp";

                    // Escribir seg�n el tipo de archivo
                    switch (extension)
                    {
                        case ".json":
                            CrearArchivoJSON(archivoTemporal);
                            break;
                        case ".csv":
                            CrearArchivoCSV(archivoTemporal);
                            break;
                        default:
                            CrearArchivoTexto(archivoTemporal);
                            break;
                    }

                    // REMPLAZAR el archivo original con el temporal
                    if (File.Exists(archivoActual))
                    {
                        File.Delete(archivoActual);
                    }
                    File.Move(archivoTemporal, archivoActual);

                    MessageBox.Show("Archivo modificado exitosamente.",
                                   "�xito",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);

                    // actualizar el t�tulo del formulario
                    this.Text = $"Archivos Secuenciales - {Path.GetFileName(archivoActual)} [Modificado]";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el archivo: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            AbrirArchivo();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ModificarArchivo();
        }

        private void CrearCarpeta()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Seleccione la ubicaci�n donde crear la carpeta";

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string ubicacionPadre = folderBrowserDialog1.SelectedPath;

                    // Solicitar el nombre de la nueva carpeta
                    string nombreCarpeta = Microsoft.VisualBasic.Interaction.InputBox(
                        "Ingrese el nombre de la nueva carpeta:",
                        "Crear Carpeta",
                        "Nueva Carpeta");

                    if (string.IsNullOrWhiteSpace(nombreCarpeta))
                    {
                        MessageBox.Show("Debe ingresar un nombre v�lido para la carpeta.",
                                       "Advertencia",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }

                    string rutaCarpeta = Path.Combine(ubicacionPadre, nombreCarpeta);

                    if (Directory.Exists(rutaCarpeta))
                    {
                        MessageBox.Show("La carpeta ya existe en esta ubicaci�n.",
                                       "Advertencia",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }

                    Directory.CreateDirectory(rutaCarpeta);
                    MessageBox.Show($"Carpeta creada exitosamente:\n{rutaCarpeta}",
                                   "�xito",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la carpeta: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void RenombrarArchivoOCarpeta()
        {
            try
            {
                // Preguntar si es archivo o carpeta
                DialogResult tipoSeleccion = MessageBox.Show(
                    "�Desea renombrar un archivo?\n\nS� = Archivo\nNo = Carpeta",
                    "Seleccionar tipo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (tipoSeleccion == DialogResult.Cancel)
                {
                    return;
                }

                string rutaOriginal = "";
                string nombreOriginal = "";
                bool esArchivo = tipoSeleccion == DialogResult.Yes;

                if (esArchivo)
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = "Todos los archivos (*.*)|*.*|" + FILTRO_ARCHIVOS;
                    openFileDialog1.Title = "Seleccionar archivo a renombrar";

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        rutaOriginal = openFileDialog1.FileName;
                        nombreOriginal = Path.GetFileName(rutaOriginal);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                    folderBrowserDialog1.Description = "Seleccione la carpeta a renombrar";

                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        rutaOriginal = folderBrowserDialog1.SelectedPath;
                        nombreOriginal = Path.GetFileName(rutaOriginal);
                    }
                    else
                    {
                        return;
                    }
                }

                // Solicitar el nuevo nombre
                string nuevoNombre = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Ingrese el nuevo nombre para: {nombreOriginal}",
                    "Renombrar",
                    nombreOriginal);

                if (string.IsNullOrWhiteSpace(nuevoNombre))
                {
                    MessageBox.Show("Debe ingresar un nombre v�lido.",
                                   "Advertencia",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    return;
                }

                string directorioPadre = Path.GetDirectoryName(rutaOriginal);
                string rutaNueva = Path.Combine(directorioPadre, nuevoNombre);

                // Verificar si ya existe
                if ((esArchivo && File.Exists(rutaNueva)) || (!esArchivo && Directory.Exists(rutaNueva)))
                {
                    MessageBox.Show("Ya existe un elemento con ese nombre en la ubicaci�n.",
                                   "Advertencia",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    return;
                }

                // Renombrar
                if (esArchivo)
                {
                    File.Move(rutaOriginal, rutaNueva);

                    // Actualizar archivoActual si era el archivo que estaba abierto
                    if (archivoActual == rutaOriginal)
                    {
                        archivoActual = rutaNueva;
                        this.Text = $"Archivos Secuenciales - {Path.GetFileName(rutaNueva)}";
                    }
                }
                else
                {
                    Directory.Move(rutaOriginal, rutaNueva);
                }

                MessageBox.Show($"Renombrado exitosamente a:\n{nuevoNombre}",
                               "�xito",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al renombrar: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void AgregarInformacionAlArchivo()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = FILTRO_ARCHIVOS;
                openFileDialog1.Title = "Seleccionar archivo para agregar informaci�n";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = openFileDialog1.FileName;
                    string extension = Path.GetExtension(rutaArchivo).ToLower();

                    // Solicitar la informaci�n a agregar
                    string informacion = Microsoft.VisualBasic.Interaction.InputBox(
                        "Ingrese la informaci�n que desea agregar al archivo:",
                        "Agregar Informaci�n",
                        "");

                    if (string.IsNullOrWhiteSpace(informacion))
                    {
                        MessageBox.Show("No se ingres� informaci�n para agregar.",
                                       "Advertencia",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }

                    // Agregar seg�n el tipo de archivo
                    switch (extension)
                    {
                        case ".json":
                            AgregarAJSON(rutaArchivo, informacion);
                            break;
                        case ".csv":
                            AgregarACSV(rutaArchivo, informacion);
                            break;
                        default:
                            AgregarATexto(rutaArchivo, informacion);
                            break;
                    }

                    MessageBox.Show("Informaci�n agregada exitosamente al archivo.",
                                   "�xito",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);

                    // Si es el archivo actual, recargar el contenido
                    if (archivoActual == rutaArchivo)
                    {
                        dgvDatos.Rows.Clear();
                        switch (extension)
                        {
                            case ".json":
                                LeerArchivoJSON(rutaArchivo);
                                break;
                            case ".csv":
                                LeerArchivoCSV(rutaArchivo);
                                break;
                            default:
                                LeerArchivoTexto(rutaArchivo);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar informaci�n al archivo: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void AgregarATexto(string rutaArchivo, string informacion)
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivo, append: true))
            {
                writer.WriteLine(informacion);
            }
        }

        private void AgregarACSV(string rutaArchivo, string informacion)
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivo, append: true))
            {
                if (informacion.Contains(',') || informacion.Contains('"') || informacion.Contains('\n'))
                {
                    informacion = $"\"{informacion.Replace("\"", "\"\"")}\"";
                }
                writer.WriteLine(informacion);
            }
        }

        private void AgregarAJSON(string rutaArchivo, string informacion)
        {
            List<string> datos = new List<string>();

            // Leer el JSON existente
            if (File.Exists(rutaArchivo))
            {
                string contenido = File.ReadAllText(rutaArchivo);
                try
                {
                    datos = System.Text.Json.JsonSerializer.Deserialize<List<string>>(contenido) ?? new List<string>();
                }
                catch
                {
                    datos = new List<string>();
                }
            }

            // Agregar el nuevo elemento
            datos.Add(informacion);

            // Guardar el JSON actualizado
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(datos, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(rutaArchivo, jsonContent);
        }

        private void EliminarInformacionDelArchivo()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = FILTRO_ARCHIVOS;
                openFileDialog1.Title = "Seleccionar archivo para eliminar informaci�n";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = openFileDialog1.FileName;
                    string extension = Path.GetExtension(rutaArchivo).ToLower();

                    // Leer todas las l�neas del archivo
                    List<string> lineas = new List<string>();

                    switch (extension)
                    {
                        case ".json":
                            string contenido = File.ReadAllText(rutaArchivo);
                            try
                            {
                                lineas = System.Text.Json.JsonSerializer.Deserialize<List<string>>(contenido) ?? new List<string>();
                            }
                            catch
                            {
                                MessageBox.Show("El archivo JSON no tiene el formato esperado.",
                                              "Error",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                                return;
                            }
                            break;
                        default:
                            using (StreamReader reader = new StreamReader(rutaArchivo))
                            {
                                string linea;
                                while ((linea = reader.ReadLine()) != null)
                                {
                                    // Para CSV, remover comillas si est�n presentes
                                    if (extension == ".csv" && linea.StartsWith("\"") && linea.EndsWith("\""))
                                    {
                                        linea = linea.Substring(1, linea.Length - 2).Replace("\"\"", "\"");
                                    }
                                    lineas.Add(linea);
                                }
                            }
                            break;
                    }

                    if (lineas.Count == 0)
                    {
                        MessageBox.Show("El archivo est� vac�o.",
                                       "Informaci�n",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                        return;
                    }

                    // Mostrar las l�neas con numeraci�n
                    string contenidoMostrar = "L�neas del archivo:\n\n";
                    for (int i = 0; i < lineas.Count; i++)
                    {
                        contenidoMostrar += $"{i + 1}. {lineas[i]}\n";
                    }

                    MessageBox.Show(contenidoMostrar, "Contenido del archivo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Solicitar el n�mero de l�nea a eliminar
                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Ingrese el n�mero de l�nea a eliminar (1-{lineas.Count}):\n\nPara eliminar m�ltiples l�neas, sep�relas con comas.\nEjemplo: 1,3,5",
                        "Eliminar L�neas",
                        "");

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return;
                    }

                    // Procesar los n�meros de l�nea
                    string[] numerosStr = input.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List<int> lineasAEliminar = new List<int>();

                    foreach (string numeroStr in numerosStr)
                    {
                        if (int.TryParse(numeroStr.Trim(), out int numeroLinea))
                        {
                            if (numeroLinea >= 1 && numeroLinea <= lineas.Count)
                            {
                                lineasAEliminar.Add(numeroLinea - 1); // Convertir a �ndice base 0
                            }
                        }
                    }

                    if (lineasAEliminar.Count == 0)
                    {
                        MessageBox.Show("No se ingresaron n�meros de l�nea v�lidos.",
                                       "Advertencia",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }

                    // Confirmar eliminaci�n
                    DialogResult resultado = MessageBox.Show(
                        $"�Est� seguro de eliminar {lineasAEliminar.Count} l�nea(s)?",
                        "Confirmar eliminaci�n",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        // Ordenar en orden descendente para eliminar correctamente
                        lineasAEliminar.Sort();
                        lineasAEliminar.Reverse();

                        foreach (int indice in lineasAEliminar)
                        {
                            lineas.RemoveAt(indice);
                        }

                        // Reescribir el archivo seg�n su tipo
                        switch (extension)
                        {
                            case ".json":
                                string jsonContent = System.Text.Json.JsonSerializer.Serialize(lineas, new System.Text.Json.JsonSerializerOptions
                                {
                                    WriteIndented = true,
                                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                                });
                                File.WriteAllText(rutaArchivo, jsonContent);
                                break;
                            case ".csv":
                                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                                {
                                    foreach (string linea in lineas)
                                    {
                                        string valor = linea;
                                        if (valor.Contains(',') || valor.Contains('"') || valor.Contains('\n'))
                                        {
                                            valor = $"\"{valor.Replace("\"", "\"\"")}\"";
                                        }
                                        writer.WriteLine(valor);
                                    }
                                }
                                break;
                            default:
                                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                                {
                                    foreach (string linea in lineas)
                                    {
                                        writer.WriteLine(linea);
                                    }
                                }
                                break;
                        }

                        MessageBox.Show($"{lineasAEliminar.Count} l�nea(s) eliminada(s) exitosamente.",
                                       "�xito",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);

                        // Si es el archivo actual, recargar el contenido
                        if (archivoActual == rutaArchivo)
                        {
                            dgvDatos.Rows.Clear();
                            foreach (string linea in lineas)
                            {
                                dgvDatos.Rows.Add(linea);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar informaci�n del archivo: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void BuscarRegistroPorID(string rutaDatos, int idBuscado)
        {
            string rutaIndice = Path.ChangeExtension(rutaDatos, ".idx");

            if (!File.Exists(rutaIndice))
            {
                MessageBox.Show("No se encontró el archivo de índice.");
                return;
            }

            using (BinaryReader readerIndice = new BinaryReader(File.Open(rutaIndice, FileMode.Open)))
            using (BinaryReader readerDatos = new BinaryReader(File.Open(rutaDatos, FileMode.Open)))
            {
                bool encontrado = false;
                while (readerIndice.BaseStream.Position < readerIndice.BaseStream.Length)
                {
                    int id = readerIndice.ReadInt32();
                    long posicion = readerIndice.ReadInt64();

                    if (id == idBuscado)
                    {
                        // Salto directo a la posición en el archivo de datos
                        readerDatos.BaseStream.Seek(posicion, SeekOrigin.Begin);
                        string contenido = readerDatos.ReadString();

                        MessageBox.Show($"ID: {id}\nInformación: {contenido}", "Registro Encontrado");
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    MessageBox.Show("El ID buscado no existe en el índice.");
                }
            }
        }

        private void btnCrearCarpeta_Click(object sender, EventArgs e)
        {
            CrearCarpeta();
        }

        private void btnRenombrar_Click(object sender, EventArgs e)
        {
            RenombrarArchivoOCarpeta();
        }

        private void btnAgregarInfo_Click(object sender, EventArgs e)
        {
            AgregarInformacionAlArchivo();
        }

        private void btnEliminarInfo_Click(object sender, EventArgs e)
        {
            EliminarInformacionDelArchivo();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(archivoActual))
            {
                MessageBox.Show("Primero abra o cree un archivo.");
                return;
            }

            // Usamos el InputBox para pedir el ID
            string input = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el ID a buscar:", "Búsqueda Indexada", "1");

            if (int.TryParse(input, out int id))
            {
                BuscarRegistroPorID(archivoActual, id);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número de ID válido.");
            }
        }
    }
}
