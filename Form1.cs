namespace Conversor_litendian_puntoflotante;

public partial class Form1 : Form
{
    private TextBox txtNumero;
    private Label lblBigEndian;
    private Label lblBinario;
    private Label lblLittleEndian;
    private Button btnConversor;

    public Form1()
    {
        txtNumero = new TextBox();
        lblBigEndian = new Label();
        lblBinario = new Label();
        lblLittleEndian = new Label();
        btnConversor = new Button();

        // Configuración del campo de entrada para el número decimal
        txtNumero.Location = new System.Drawing.Point(30, 30);
        txtNumero.Width = 200;
        txtNumero.PlaceholderText = "Introduce un número";

        // Etiqueta para mostrar Little Endian
        lblLittleEndian.Location = new System.Drawing.Point(30, 80);
        lblLittleEndian.Width = 400;
        lblLittleEndian.Text = "Little Endian: ";

        //Etiqueta para mostrar Big Endian
        lblBigEndian.Location = new System.Drawing.Point(30, 130);
        lblBigEndian.Width = 400;
        lblBigEndian.Text = "Big Endian: ";

        // Etiqueta para mostrar binario
        lblBinario.Location = new System.Drawing.Point(30, 180);
        lblBinario.Width = 400;
        lblBinario.Text = "Binario: ";

        // Botón para convertir
        btnConversor.Text = "Conversor";
        btnConversor.Location = new System.Drawing.Point(30, 230);
        btnConversor.Click += BtnConversor_Click;

        // Añadir controles al formulario
        Controls.Add(txtNumero);
        Controls.Add(lblLittleEndian);
        Controls.Add(lblBigEndian);
        Controls.Add(lblBinario);
        Controls.Add(btnConversor);

        // Configuración del formulario
        Text = "Conversor Little Endian / Big Endian / Binario";
        Width = 450;
        Height = 350;
    }

    // Método que se ejecuta al hacer clic en el botón
    private void BtnConversor_Click(object sender, EventArgs e)
    {
        if (double.TryParse(txtNumero.Text, out double numero))
        {
            // Convertir el número a binario (con punto flotante)
            string binario = Decimal_binario(numero);
            lblBinario.Text = $"Binario: {binario}";
            // floatBytes darle formato de half presition S|EXP|Mantissa de 10 bits 
            //var (mantisa, exp) = NormalizarBinario(binario);
            //string mantisaExp = SumExpMant(mantisa, exp);
            //int valorBits = Convert.ToInt32(mantisaExp, 2); 
            
            byte[] floatBytes = BitConverter.GetBytes(numero);
            // Convertir a Big Endian
            byte[] bigEndianBytes = floatBytes.Reverse().ToArray();
            string bigEndian = BitConverter.ToString(bigEndianBytes);
            lblBigEndian.Text = $"Big Endian: {bigEndian}";

            // Mostrar Little Endian (el orden predeterminado en BitConverter es Little Endian)
            string littleEndian = BitConverter.ToString(floatBytes);
            lblLittleEndian.Text = $"Little Endian: {littleEndian}";
        }
        else
        {
            MessageBox.Show("Por favor, introduce un número válido.");
        }
    }
    
    //
    static string Decimal_binario(double numero)
    {

        int formHalf = 16;
        int parteEntera;
        double parteFraccion;

        string bitsEntero = "";
        string bitsFraccion = "";

            parteEntera = (int)(numero);
            parteFraccion = numero - (double)(parteEntera);

            while(parteEntera != 0)// Si la parte entera tiene un valor mayor a cero, se convierte en bits
            {
                if(parteEntera%2 == 0)
                {
                    bitsEntero += "0";
                }
                else
                {
                    bitsEntero += "1";
                }
                parteEntera /= 2;
            }

            for (int i = 0; i < formHalf ; i++)
            {
                parteFraccion *= 2;
                if(parteFraccion < 1)
                {
                    bitsFraccion += "0";
                }
                else
                {
                    bitsFraccion += "1";
                    parteFraccion -= 1;
                }
            }

            bitsEntero = InvertirString(bitsEntero);

            return bitsEntero+"."+bitsFraccion;
    }

    static string InvertirString(string dato)
    {
        string auxiliar = dato;
        dato = "";

        for(int i = auxiliar.Length - 1 ; i>=0 ; i--)
        {
            dato += auxiliar[i];
        }
        return dato;
    }

    // Método que normaliza un número binario en formato de cadena con parte flotante
    /*static (string, int) NormalizarBinario(string binario)
    {
        // Buscar la posición del punto decimal
        int puntoPos = binario.IndexOf('.');
        if (puntoPos == -1) puntoPos = binario.Length; // Si no tiene punto, asumir al final

        // Eliminar el punto decimal para facilitar los cálculos
        binario = binario.Replace(".", "");

        // Buscar el primer '1'
        int primer1Pos = binario.IndexOf('1');
        if (primer1Pos == -1)
        {
            return ("0", 0); // No hay '1', el número es 0
        }

        // Normalizar el binario (mover el punto después del primer '1')
        string mantisa = binario.Substring(primer1Pos + 1); // Lo que queda después del primer '1'

        // Calcular el exponente (posiciones movidas del punto original)
        int exponente = puntoPos - (primer1Pos + 1);

        // Retornar la mantisa normalizada y el exponente
        return (mantisa, exponente);
    }

    // Método que suma la mantisa y el exponente de un número binario en formato de cadena
    static string SumExpMant(string mantisa, int exponente)
    {
        // Seleccionamos los primeros 5 bits de la mantisa
        string primeros5Bits = mantisa.Substring(0, Math.Min(5, mantisa.Length));

        // Convertimos los primeros 5 bits a decimal, les sumamos el exponente
        int valorBits = Convert.ToInt32(primeros5Bits, 2);
        valorBits += exponente;

        // Convertimos de nuevo a binario, rellenando hasta 5 bits
        string resultadoBinario = Convert.ToString(valorBits, 2).PadLeft(5, '0');

        return resultadoBinario;
    }*/

}