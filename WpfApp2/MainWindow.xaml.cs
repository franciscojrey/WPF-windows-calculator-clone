using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{

    public partial class MainWindow : Window
    {
  
        decimal resultado;
        string operador = "+";
        bool clickBotonOperador;
        bool hizoElCalculo; // Agregué esta variable para que cuando se hace un cálculo y se modifica el TextBox,
                            // no ponga "clickBotonOperador = false;" porque sino volvés a tocar un operador y sigue haciendo las cuentas.
        bool vaciarTxtBox = true;
        bool errorEnOperacion;
        bool limpiarTxtBlock;
        bool presionoBotonIgual;
        decimal ultimoNumero;

        bool botonComaPresionado;

        bool hagoUnaOperacion;
        bool noModificar;


        public MainWindow()
        {
            InitializeComponent();
            txtBoxNumero.Text = "0";
            txtBoxNumero.Focus();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            chequeaSiBorraValores();
            if (txtBoxNumero.Text == "0")
            {
                txtBoxNumero.Text = ((Button)sender).Content.ToString();
            }
            else
            {
                txtBoxNumero.Text += ((Button)sender).Content.ToString(); 
            }
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            hagoUnaOperacion = true;
            noModificar = false;

            presionoBotonIgual = false;
            limpiarTxtBlock = false;
            if (clickBotonOperador == false) // Para poder cambiar de operador y que no calcule cada vez que toco un operador, porque quizás quiero cambiar de + a -
                                             // sin hacer la cuenta.
                                             // Solo entra si toqué un botón de número antes o lo modifiqué a mano, sino solo cambia el operador.
            {
                clickBotonOperador = true;
                Calcular(operador);
            }

            txtBlockCuenta.Text = resultado.ToString() + ' ' + operador; // Ver calculadora de Windows. Tendría que ser distinto si da error, por ej si divido 0 / 0

            if (errorEnOperacion == false)
            {
                txtBoxNumero.Text = resultado.ToString();
            }
            else
            {
                errorEnOperacion = false;
            }

            operador = ((Button)sender).Content.ToString();
            vaciarTxtBox = true;
            botonComaPresionado = false;
            ReemplazarOperadorTextBlock();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
           if (vaciarTxtBox && limpiarTxtBlock)
            {
                vaciarTxtBox = false;
                limpiarTxtBlock = false;
                txtBoxNumero.Text = "0";
            }

            if (presionoBotonIgual == true && noModificar == false)
            {
                txtBlockCuenta.Text = txtBoxNumero.Text + " " + operador + " " + ultimoNumero + " =";
            }
            else
            {
                if (noModificar == true)
                {
                    txtBlockCuenta.Text = " " + txtBoxNumero.Text + " =";
                } else
                {
                    txtBlockCuenta.Text += " " + txtBoxNumero.Text + " =";
                }
                ultimoNumero = Decimal.Parse(txtBoxNumero.Text);
            }

            if (noModificar == false)
            {
                Calcular(operador);
            }

            if (errorEnOperacion == false)
            {
                txtBoxNumero.Text = resultado.ToString();
                limpiarTxtBlock = false;
                presionoBotonIgual = true;
            }
            else
            {
                errorEnOperacion = false;
                limpiarTxtBlock = true;
                txtBoxNumero.Text = "No se puede dividir por 0.";
                resultado = 0;
                operador = "+";
            }

            if (hagoUnaOperacion == false)
            {
                noModificar = true;
            }

            vaciarTxtBox = true; 
            clickBotonOperador = true;
            botonComaPresionado = false;
        }

        private void btnBorrarUno_Click(object sender, RoutedEventArgs e)
        {

            // Ál hacer el metodo de abajo borra todo aunque estés borrando de a uno después de operar. Ver si es la mejor forma de hacerlo
            // o conviene que funcione de otra manera
            chequeaSiBorraValores();
            if (txtBoxNumero.Text != "0" && txtBoxNumero.Text != "")
            {
                if (txtBoxNumero.Text[txtBoxNumero.Text.Length - 1].ToString() == ",")
                { // Si el ultimo caracter es la coma y la borra puede volver a ingresarla:                
                    botonComaPresionado = false;
                }
                txtBoxNumero.Text = txtBoxNumero.Text.Remove(txtBoxNumero.Text.Length - 1);
            }
            limpiarTxtBlock = false;
            vaciarTxtBox = false;
        }

        private void btnBorrarTodo_Click(object sender, RoutedEventArgs e)
        {
            txtBoxNumero.Text = "0";
            txtBlockCuenta.Text = String.Empty;
            resultado = 0;
            vaciarTxtBox = true;
            limpiarTxtBlock = false;
            operador = "+";
            botonComaPresionado = false;
            hagoUnaOperacion = false;
            noModificar = false;
        }

        private void Calcular(string operador)
        {
            switch (operador)
            {
                case "+":
                    if (presionoBotonIgual)
                    {
                        resultado = ultimoNumero + Decimal.Parse(txtBoxNumero.Text);
                    }
                    else
                    {
                        resultado += Decimal.Parse(txtBoxNumero.Text);
                    }
                    hizoElCalculo = true;
                    break;
                case "-":
                    if (presionoBotonIgual)
                    {
                        resultado = Decimal.Parse(txtBoxNumero.Text) - ultimoNumero;  // Ver que pasa si hago 2 - 4, presiono "=" un par de veces, toco "-" y toco "=". Se dan vuelta los signos
                    }
                    else
                    {
                        resultado -= Decimal.Parse(txtBoxNumero.Text);
                    }
                    hizoElCalculo = true;
                    break;
                case "*":
                    if (presionoBotonIgual)
                    {
                        resultado = ultimoNumero * Decimal.Parse(txtBoxNumero.Text);
                    }
                    else
                    {
                        resultado *= Decimal.Parse(txtBoxNumero.Text);
                    }
                    hizoElCalculo = true;
                    break;
                case "/":
                    if (presionoBotonIgual)
                    {
                        try
                        {
                            resultado = Decimal.Parse(txtBoxNumero.Text) / ultimoNumero;
                        }
                        catch (System.DivideByZeroException)
                        {
                            txtBoxNumero.Text = "No se puede dividir por 0."; // Ver que en la calculadora de Windows es distinto el error si hacés 0 / 0 o 9 / 0.
                            errorEnOperacion = true;
                        }                        
                    }
                    else
                    {                        
                        try
                        {
                            resultado /= Decimal.Parse(txtBoxNumero.Text);
                        }
                        catch (System.DivideByZeroException)
                        {
                            txtBoxNumero.Text = "No se puede dividir por 0."; // Ver que en la calculadora de Windows es distinto el error si hacés 0 / 0 o 9 / 0.
                            errorEnOperacion = true;
                        }
                    }                           
                    hizoElCalculo = true;
                    break;
            }
        }

        private void txtBoxNumero_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Para que después de tocar un operador se haga el cálculo, solo si cambio el Textbox.
            chequeaClickAnteriorEnOperador();
            //if (txtBoxNumero.Text == "" && vaciarTxtBox == true)
            if (txtBoxNumero.Text == "")
            {
                txtBoxNumero.Text = "0";
            }
        }

        private void chequeaClickAnteriorEnOperador()
        {
            // Ver el click de los operadores. Sirve para que cuando después de tocar los botones, si toque en algún operador, haga la cuenta. Si no tocó ningún número antes
            // no la hace y solo cambia el operador
            if (clickBotonOperador && !hizoElCalculo)
            {
                clickBotonOperador = false;
            }
            if (hizoElCalculo)
            {
                hizoElCalculo = false;
            }
        }

        private void chequeaSiBorraValores()
        {
            if (vaciarTxtBox)
            {
                vaciarTxtBox = false;
                txtBoxNumero.Clear();                
            }
            if (limpiarTxtBlock)
            {
                txtBlockCuenta.Text = String.Empty;
                resultado = 0;
                limpiarTxtBlock = false;
            }
            if (presionoBotonIgual == true)
            {
                hagoUnaOperacion = false;
                noModificar = false;
                txtBlockCuenta.Text = String.Empty;
                resultado = 0;
                presionoBotonIgual = false;
            }
        }

        private void ReemplazarOperadorTextBlock()
        {
            txtBlockCuenta.Text = txtBlockCuenta.Text.Remove(txtBlockCuenta.Text.Length - 1) + operador;
        }

        private void btnComa_Click(object sender, RoutedEventArgs e)
        {
            if (botonComaPresionado == false)
            {
                txtBoxNumero.Text += ",";
                vaciarTxtBox = false;
                botonComaPresionado = true;
                limpiarTxtBlock = false;
            }

        }

        private void btnChangeSign_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxNumero.Text.StartsWith("-"))
            {
                txtBoxNumero.Text = txtBoxNumero.Text.Remove(0, 1);
            }
            else
            {
                txtBoxNumero.Text = txtBoxNumero.Text.Insert(0, "-");
            }
        }
    }
}