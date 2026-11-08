using System.Drawing;
using System.Windows.Forms;

namespace Tyuiu.KAV.Sprint7.Project.V7
{
    public class FormHelpUserGuide_KAV : Form
    {
        public FormHelpUserGuide_KAV()
        {
            this.Text = "Краткое руководство пользователя";
            this.Size = new Size(520, 380);
            this.StartPosition = FormStartPosition.CenterParent;

            var textBoxManualInfo_KAV = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9.5f),
                Text = "ИНСТРУКЦИЯ ПО РАБОТЕ С СИСТЕМОЙ:\r\n\r\n" +
                       "1. Загрузка и сохранение данных:\r\n" +
                       "   - Файл -> Открыть CSV или кнопка на панели инструментов.\r\n" +
                       "   - Данные сохраняются автоматически или через пункт меню 'Сохранить'.\r\n\r\n" +
                       "2. Поиск и фильтрация:\r\n" +
                       "   - Введите фамилию или примечание в поле 'Поиск' для быстрой выборки.\r\n" +
                       "   - Выберите фильтр по подъездам или задолженности в верхней панели.\r\n\r\n" +
                       "3. Добавление и изменение:\r\n" +
                       "   - Заполните поля ввода в правой панели и нажмите 'Добавить'.\r\n" +
                       "   - Чтобы изменить запись, выберите строку в таблице, отредактируйте поля и нажмите 'Изменить'.\r\n\r\n" +
                       "4. Анализ и статистика:\r\n" +
                       "   - Вкладка 'Статистика и графики' отображает сводные показатели (среднее, min/max) и гистограмму площадей по квартирам."
            };

            this.Controls.Add(textBoxManualInfo_KAV);
        }
    }
}
