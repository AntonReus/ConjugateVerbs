using System;
using System.Windows.Forms;
using LinqToExcel;
using System.Data;
using System.Linq;


namespace ConjugateVerbs
{
    public partial class frmPS : Form
    {
        int errores = 0;
        int correct = 0;
        int intentos = 0;

        string verbo;
        string subjet;

        public frmPS()
        {
            InitializeComponent();
        }

        private void frmPS_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Complete the conjugation of the verb with s, es or ies as appropriate");
            lblSentence.Text = sentences();

        }
        public void Search_verb()
        {
            Random rdn = new Random();
            string id_verb = Convert.ToString(rdn.Next(1, 294));


            //Ruta del Excel
            string ruta = Application.StartupPath + "\\Verbos.xlsx";
            //OT0RGANDO LAREFERENCIA DEL ARCHIVO
            var book = new ExcelQueryFactory(ruta);

            //EXTRAER SOLO LA INFORMACION DE UN BASTARDO

            var resultado = (from row in book.Worksheet("Hoja1")
                             where (string)row["Id_Verbo"] == id_verb
                             let item = new cVerbo
                             {

                                 id_verb = row[0].Cast<string>(),
                                 Verb = row[1].Cast<string>(),

                             }

                             select item).ToList();
            book.Dispose();


            //Mostrar los Datos

            foreach (var item in resultado)
            {

                id_verb = item.id_verb;
                verbo = item.Verb;

            }
        }

        public void Search_subjet()
        {

            Random rdn = new Random();
            string id_subjet = Convert.ToString(rdn.Next(1, 5));


            //Ruta del Excel
            string ruta = Application.StartupPath + "\\Verbos.xlsx";
            //OT0RGANDO LAREFERENCIA DEL ARCHIVO
            var book = new ExcelQueryFactory(ruta);

            //EXTRAER SOLO LA INFORMACION DE UN BASTARDO

            var resultado = (from row in book.Worksheet("Hoja2")
                             where (string)row["Id_subjet"] == id_subjet
                             let item = new cSubjet
                             {

                                 id_subjet = row[0].Cast<string>(),
                                 Subjet = row[1].Cast<string>(),

                             }

                             select item).ToList();
            book.Dispose();


            //Mostrar los Datos

            foreach (var item in resultado)
            {

                id_subjet = item.id_subjet;
                subjet = item.Subjet;

            }
        }
        public string sentences()
        {
            Search_verb();
            Search_subjet();
            lblVerb.Text = verbo;
            return subjet + " " + verbo;
        }
        public int SVerb()
        {
            int letras = verbo.Length;

            if (verbo[letras - 1] == 's' || verbo[letras - 1] == 'h' || verbo[letras - 1] == 'o' || verbo[letras - 1] == 'x')
            {
                return 3;
            }
            else if (verbo[letras - 1] == 'y')
            {

                if (verbo[letras - 2] == 'a' || verbo[letras - 2] == 'e' || verbo[letras - 2] == 'e' || verbo[letras - 2] == 'i' || verbo[letras - 2] == 'u')
                {
                    return 2;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                return 2;
            }
        }
        public bool Isthird()
        {
            return (subjet == "He" || subjet == "She");
        }
        public bool Check()
        {
            //LAS EXPRECIONES REGRULARES DE LA LIBRERIA REGEX DE C# NO SON COMPATIBLES

            if (Isthird())
            {
                switch (SVerb())
                {
                    case 2:
                        if (txtConju.Text == "s")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case 3:
                        if (txtConju.Text == "es")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case 4:
                        if (txtConju.Text == "ies")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                }
                return false;
            }
            else
            {
                if (txtConju.Text == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            intentos++;
            lblTotal.Text = Convert.ToString(intentos);
            if (Check())
            {
                MessageBox.Show("Well done");
                correct++;
                lblCorrect.Text = Convert.ToString(correct);
            }
            else
            {
                MessageBox.Show("Wrong");
                errores++;
                lblWrong.Text = Convert.ToString(errores);

            }

            lblSentence.Text = sentences();

            txtConju.Clear();
        }
    }
}
