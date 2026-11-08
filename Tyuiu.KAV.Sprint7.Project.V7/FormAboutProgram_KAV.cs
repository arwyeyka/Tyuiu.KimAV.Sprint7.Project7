using System.Drawing;
using System.Windows.Forms;

namespace Tyuiu.KAV.Sprint7.Project.V7
{
    public class FormAboutProgram_KAV : Form
    {
        public FormAboutProgram_KAV()
        {
            this.Text = "О программе — Домоуправление";
            this.Size = new Size(440, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var labelInfoDeveloper_KAV = new Label
            {
                Text = "Информационная система «Домоуправление»\n\n" +
                       "Разработчик: К.А.В. (Sprint 7, Вариант 7)\n" +
                       "Тюменский индустриальный университет (ТИУ)\n\n" +
                       "Программа предназначена для ведения учета жилого фонда,\n" +
                       "квартиросъемщиков, задолженностей и формирования статистики.",
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                Font = new Font("Segoe UI", 9.5f)
            };

            var buttonCloseDialog_KAV = new Button
            {
                Text = "ОК",
                Dock = DockStyle.Bottom,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            this.Controls.Add(labelInfoDeveloper_KAV);
            this.Controls.Add(buttonCloseDialog_KAV);
        }
    }
}
