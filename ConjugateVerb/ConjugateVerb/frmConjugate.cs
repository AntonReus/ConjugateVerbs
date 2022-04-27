using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqToExcel;
using System.Data.OleDb;

namespace ConjugateVerb
{
    public partial class frmConjugate : Form
    {
        int errores = 0;
        int correct = 0;
        int intento = 0;

        string verbo;
        string subject;

        public frmConjugate()
        {
            InitializeComponent();
        }

        private void frmConjugate_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Complete the conjugation");
            lblSentence.Text = sentences();
        }

        public void Search_Verb ()
        {
            Random rnd = new Random();
            string id_verb = Convert.ToString(rnd.Next(1,294));

            string ruta = Application.StartupPath + "\\Verbos.xlsx";

            var book = new ExcelQueryFactory(ruta);

            var resultado = (from row in book.Worksheet("Hoja1")
                             where (string)row["Id_Verbo"] == id_verb
                             let item = new cVerbo
                             {
                                 id_verb = row[0].Cast<string>(),
                                 Verb = row[1].Cast<string>(),
                             }

                             select item).ToList();
            book.Dispose();

            foreach (var item in resultado)
            {
                id_verb = item.id_verb;
                verbo = item.Verb;
            }
        }

        public void Search_Subject()
        {
            Random rdn = new Random();
            string id_subject = Convert.ToString(rdn.Next(1, 5));

            string ruta = Application.StartupPath + "\\Verbos.xlsx";
            
            var book = new ExcelQueryFactory (ruta);

            var resultado = (from row in book.Worksheet("Hoja2")
                             where (string)row["Id_subject"] == id_subject
                             let item = new cSubject
                             {
                                 id_subject = row[0].Cast<string>(),
                                 Subject = row[1].Cast<string>(),
                             }
                             select item).ToList();
            book.Dispose();

            foreach (var item in resultado)
            {
                id_subject = item.id_subject;
                subject = item.Subject;
            }
        }

        public string sentences ()
        {
            Search_Verb();
            Search_Subject();
            lblVerb.Text = verbo;
            return subject + " " + verbo;
        }

        public int SVerb()
        {
            int letras = verbo.Length;

            if (verbo[letras - 1] == 's' || verbo[letras - 1] == 'h')
            {
                return 3;
            }
            else if (verbo[letras - 1] == 'y')
            {
                if (verbo[letras - 1] == 'a' || verbo[letras - 1] == 'e' || verbo[letras - 1] == 'i' || verbo[letras - 1] == 'o' || verbo[letras - 1] == 'u')
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

        public bool IsThird ()
        {
            return (subject == "He" || subject == "She");
        }

        public bool Check ()
        {
            if (IsThird())
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
                if(txtConju.Text == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            intento++;
            lblTotal.Text = Convert.ToString(intento);
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
