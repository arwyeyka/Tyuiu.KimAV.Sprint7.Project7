using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Tyuiu.KAV.Sprint7.Project.V7.Lib;

namespace Tyuiu.KAV.Sprint7.Project.V7
{
    public class FormMain_KAV : Form
    {
        private readonly DataService _dataService = new DataService();
        private BindingList<ApartmentRecord> _apartments = new BindingList<ApartmentRecord>();
        private string _activeFilePath = "apartments_data.csv";

        private MenuStrip menuStripControlMain_KAV;
        private ToolStrip toolStripActionsTool_KAV;
        private StatusStrip statusStripAppState_KAV;
        private ToolStripStatusLabel statusLabelDisplayCounter_KAV;
        private ToolTip toolTipShowHint_KAV;

        private Panel panelTopFiltersContainer_KAV;
        private TextBox textBoxSearchByOwner_KAV;
        private ComboBox comboBoxFilterEntrance_KAV;
        private ComboBox comboBoxFilterDebt_KAV;
        private ComboBox comboBoxSortParameters_KAV;
        private Button buttonResetFilters_KAV;

        private TabControl tabControlWorkArea_KAV;
        private TabPage tabPageDisplayGrid_KAV;
        private TabPage tabPageDisplayChartAndStats_KAV;

        private DataGridView dataGridViewApartmentsList_KAV;
        private Chart chartDisplayDistribution_KAV;
        private GroupBox groupBoxStatsOverview_KAV;
        private Label labelStatTotalCount_KAV;
        private Label labelStatTotalArea_KAV;
        private Label labelStatAvgArea_KAV;
        private Label labelStatMinMaxArea_KAV;
        private Label labelStatDebtors_KAV;

        private GroupBox groupBoxEditForm_KAV;
        private NumericUpDown numericUpDownInputEntrance_KAV;
        private NumericUpDown numericUpDownInputApartment_KAV;
        private NumericUpDown numericUpDownInputTotalArea_KAV;
        private NumericUpDown numericUpDownInputLivingArea_KAV;
        private NumericUpDown numericUpDownInputRooms_KAV;
        private TextBox textBoxInputOwnerLastName_KAV;
        private DateTimePicker dateTimePickerInputRegDate_KAV;
        private NumericUpDown numericUpDownInputFamilyCount_KAV;
        private NumericUpDown numericUpDownInputChildrenCount_KAV;
        private CheckBox checkBoxInputHasDebt_KAV;
        private TextBox textBoxInputNote_KAV;

        private Button buttonInsertRecord_KAV;
        private Button buttonUpdateRecord_KAV;
        private Button buttonDeleteRecord_KAV;
        private Button buttonClearInputs_KAV;

        public FormMain_KAV()
        {
            InitializeComponentManual_KAV();
            LoadInitialData_KAV();
        }

        private void InitializeComponentManual_KAV()
        {
            this.Text = "ИС «Домоуправление» — Tyuiu.KAV.Sprint7.Project.V7";
            this.Size = new Size(1250, 780);
            this.MinimumSize = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.toolTipShowHint_KAV = new ToolTip();

            menuStripControlMain_KAV = new MenuStrip();
            var menuFile = new ToolStripMenuItem("Файл");
            var menuHelp = new ToolStripMenuItem("Справка");

            var itemOpenFile = new ToolStripMenuItem("Открыть базу CSV...", null, (s, e) => ActionOpenFile_KAV());
            var itemSaveFile = new ToolStripMenuItem("Сохранить в CSV...", null, (s, e) => ActionSaveFile_KAV());
            var itemExit = new ToolStripMenuItem("Выход", null, (s, e) => this.Close());
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { itemOpenFile, itemSaveFile, new ToolStripSeparator(), itemExit });

            var itemGuide = new ToolStripMenuItem("Краткое руководство пользователя", null, (s, e) => new FormHelpUserGuide_KAV().ShowDialog());
            var itemAbout = new ToolStripMenuItem("О программе", null, (s, e) => new FormAboutProgram_KAV().ShowDialog());
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { itemGuide, itemAbout });

            menuStripControlMain_KAV.Items.AddRange(new ToolStripItem[] { menuFile, menuHelp });
            this.MainMenuStrip = menuStripControlMain_KAV;

            toolStripActionsTool_KAV = new ToolStrip();
            var btnOpenCsv = new ToolStripButton("Загрузить CSV", null, (s, e) => ActionOpenFile_KAV()) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            var btnSaveCsv = new ToolStripButton("Сохранить CSV", null, (s, e) => ActionSaveFile_KAV()) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            var btnClearAll = new ToolStripButton("Очистить поля", null, (s, e) => ClearInputs_KAV()) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            toolStripActionsTool_KAV.Items.AddRange(new ToolStripItem[] { btnOpenCsv, btnSaveCsv, new ToolStripSeparator(), btnClearAll });

            panelTopFiltersContainer_KAV = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };

            var labelSearch = new Label { Text = "Поиск (фамилия/прим.):", AutoSize = true, Left = 10, Top = 18 };
            textBoxSearchByOwner_KAV = new TextBox { Left = 160, Top = 15, Width = 150 };
            textBoxSearchByOwner_KAV.TextChanged += (s, e) => RefreshTableAndStats_KAV();
            toolTipShowHint_KAV.SetToolTip(textBoxSearchByOwner_KAV, "Фильтрация по фамилии владельца или тексту в примечании");

            var labelEntrance = new Label { Text = "Подъезд:", AutoSize = true, Left = 325, Top = 18 };
            comboBoxFilterEntrance_KAV = new ComboBox { Left = 385, Top = 15, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxFilterEntrance_KAV.Items.AddRange(new object[] { "Все подъезды", "Подъезд 1", "Подъезд 2", "Подъезд 3", "Подъезд 4" });
            comboBoxFilterEntrance_KAV.SelectedIndex = 0;
            comboBoxFilterEntrance_KAV.SelectedIndexChanged += (s, e) => RefreshTableAndStats_KAV();

            var labelDebt = new Label { Text = "Задолженность:", AutoSize = true, Left = 510, Top = 18 };
            comboBoxFilterDebt_KAV = new ComboBox { Left = 605, Top = 15, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxFilterDebt_KAV.Items.AddRange(new object[] { "Все квартиры", "С долгом", "Без долга" });
            comboBoxFilterDebt_KAV.SelectedIndex = 0;
            comboBoxFilterDebt_KAV.SelectedIndexChanged += (s, e) => RefreshTableAndStats_KAV();

            var labelSort = new Label { Text = "Сортировка:", AutoSize = true, Left = 730, Top = 18 };
            comboBoxSortParameters_KAV = new ComboBox { Left = 805, Top = 15, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxSortParameters_KAV.Items.AddRange(new object[] { "По номеру квартиры", "По фамилии (А-Я)", "По площади (возр.)", "По площади (убыв.)" });
            comboBoxSortParameters_KAV.SelectedIndex = 0;
            comboBoxSortParameters_KAV.SelectedIndexChanged += (s, e) => RefreshTableAndStats_KAV();

            buttonResetFilters_KAV = new Button { Text = "Сброс", Left = 980, Top = 14, Width = 75, Height = 25 };
            buttonResetFilters_KAV.Click += (s, e) => ResetFilters_KAV();
            toolTipShowHint_KAV.SetToolTip(buttonResetFilters_KAV, "Сбросить поисковые параметры к значениям по умолчанию");

            panelTopFiltersContainer_KAV.Controls.AddRange(new Control[] {
                labelSearch, textBoxSearchByOwner_KAV,
                labelEntrance, comboBoxFilterEntrance_KAV,
                labelDebt, comboBoxFilterDebt_KAV,
                labelSort, comboBoxSortParameters_KAV,
                buttonResetFilters_KAV
            });

            groupBoxEditForm_KAV = new GroupBox
            {
                Text = "Параметры квартиры",
                Dock = DockStyle.Right,
                Width = 320,
                Padding = new Padding(10)
            };

            int y = 25;
            numericUpDownInputEntrance_KAV = CreateNumericInput_KAV("Номер подъезда:", ref y, 1, 20);
            numericUpDownInputApartment_KAV = CreateNumericInput_KAV("Номер квартиры:", ref y, 1, 1000);
            numericUpDownInputTotalArea_KAV = CreateNumericInput_KAV("Общая площадь (кв.м):", ref y, 10, 500, 2);
            numericUpDownInputLivingArea_KAV = CreateNumericInput_KAV("Жилая площадь (кв.м):", ref y, 5, 400, 2);
            numericUpDownInputRooms_KAV = CreateNumericInput_KAV("Количество комнат:", ref y, 1, 10);

            var lblOwner = new Label { Text = "Фамилия владельца:", Left = 15, Top = y, Width = 280 };
            y += 20;
            textBoxInputOwnerLastName_KAV = new TextBox { Left = 15, Top = y, Width = 280 };
            groupBoxEditForm_KAV.Controls.Add(lblOwner);
            groupBoxEditForm_KAV.Controls.Add(textBoxInputOwnerLastName_KAV);
            y += 28;

            var lblDate = new Label { Text = "Дата прописки:", Left = 15, Top = y, Width = 280 };
            y += 20;
            dateTimePickerInputRegDate_KAV = new DateTimePicker { Left = 15, Top = y, Width = 280, Format = DateTimePickerFormat.Short };
            groupBoxEditForm_KAV.Controls.Add(lblDate);
            groupBoxEditForm_KAV.Controls.Add(dateTimePickerInputRegDate_KAV);
            y += 28;

            numericUpDownInputFamilyCount_KAV = CreateNumericInput_KAV("Членов семьи:", ref y, 1, 20);
            numericUpDownInputChildrenCount_KAV = CreateNumericInput_KAV("Детей в семье:", ref y, 0, 15);

            checkBoxInputHasDebt_KAV = new CheckBox { Text = "Имеется задолженность по ЖКУ", Left = 15, Top = y, Width = 280 };
            groupBoxEditForm_KAV.Controls.Add(checkBoxInputHasDebt_KAV);
            y += 26;

            var lblNote = new Label { Text = "Примечание:", Left = 15, Top = y, Width = 280 };
            y += 20;
            textBoxInputNote_KAV = new TextBox { Left = 15, Top = y, Width = 280 };
            groupBoxEditForm_KAV.Controls.Add(lblNote);
            groupBoxEditForm_KAV.Controls.Add(textBoxInputNote_KAV);
            y += 35;

            buttonInsertRecord_KAV = new Button { Text = "Добавить", Left = 15, Top = y, Width = 135, Height = 32 };
            buttonInsertRecord_KAV.Click += (s, e) => AddApartment_KAV();
            toolTipShowHint_KAV.SetToolTip(buttonInsertRecord_KAV, "Добавить новую запись в базу данных");

            buttonUpdateRecord_KAV = new Button { Text = "Изменить", Left = 160, Top = y, Width = 135, Height = 32 };
            buttonUpdateRecord_KAV.Click += (s, e) => UpdateApartment_KAV();
            toolTipShowHint_KAV.SetToolTip(buttonUpdateRecord_KAV, "Сохранить изменения в выбранной строке");

            buttonDeleteRecord_KAV = new Button { Text = "Удалить", Left = 15, Top = y + 38, Width = 135, Height = 32 };
            buttonDeleteRecord_KAV.Click += (s, e) => DeleteApartment_KAV();
            toolTipShowHint_KAV.SetToolTip(buttonDeleteRecord_KAV, "Удалить выбранную запись из списка");

            buttonClearInputs_KAV = new Button { Text = "Очистить форму", Left = 160, Top = y + 38, Width = 135, Height = 32 };
            buttonClearInputs_KAV.Click += (s, e) => ClearInputs_KAV();

            groupBoxEditForm_KAV.Controls.AddRange(new Control[] {
                buttonInsertRecord_KAV, buttonUpdateRecord_KAV,
                buttonDeleteRecord_KAV, buttonClearInputs_KAV
            });

            tabControlWorkArea_KAV = new TabControl { Dock = DockStyle.Fill };
            tabPageDisplayGrid_KAV = new TabPage("Таблица квартир");
            tabPageDisplayChartAndStats_KAV = new TabPage("Статистика и графики");

            dataGridViewApartmentsList_KAV = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dataGridViewApartmentsList_KAV.SelectionChanged += OnGridSelectionChanged_KAV;
            tabPageDisplayGrid_KAV.Controls.Add(dataGridViewApartmentsList_KAV);

            groupBoxStatsOverview_KAV = new GroupBox { Text = "Сводная статистика по выборке", Dock = DockStyle.Top, Height = 100, Padding = new Padding(10) };
            labelStatTotalCount_KAV = new Label { Left = 20, Top = 25, AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            labelStatTotalArea_KAV = new Label { Left = 20, Top = 55, AutoSize = true };
            labelStatAvgArea_KAV = new Label { Left = 280, Top = 25, AutoSize = true };
            labelStatMinMaxArea_KAV = new Label { Left = 280, Top = 55, AutoSize = true };
            labelStatDebtors_KAV = new Label { Left = 580, Top = 25, AutoSize = true, ForeColor = Color.DarkRed };

            groupBoxStatsOverview_KAV.Controls.AddRange(new Control[] {
                labelStatTotalCount_KAV, labelStatTotalArea_KAV,
                labelStatAvgArea_KAV, labelStatMinMaxArea_KAV, labelStatDebtors_KAV
            });

            chartDisplayDistribution_KAV = new Chart { Dock = DockStyle.Fill };
            var chartArea = new ChartArea("DefaultArea");
            chartArea.AxisX.Title = "Номер квартиры";
            chartArea.AxisY.Title = "Общая площадь (кв.м)";
            chartDisplayDistribution_KAV.ChartAreas.Add(chartArea);

            var series = new Series("Площадь квартир")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };
            chartDisplayDistribution_KAV.Series.Add(series);

            tabPageDisplayChartAndStats_KAV.Controls.Add(chartDisplayDistribution_KAV);
            tabPageDisplayChartAndStats_KAV.Controls.Add(groupBoxStatsOverview_KAV);

            tabControlWorkArea_KAV.TabPages.AddRange(new TabPage[] { tabPageDisplayGrid_KAV, tabPageDisplayChartAndStats_KAV });

            statusStripAppState_KAV = new StatusStrip();
            statusLabelDisplayCounter_KAV = new ToolStripStatusLabel("Готово. Записей: 0");
            statusStripAppState_KAV.Items.Add(statusLabelDisplayCounter_KAV);

            this.Controls.Add(tabControlWorkArea_KAV);
            this.Controls.Add(groupBoxEditForm_KAV);
            this.Controls.Add(panelTopFiltersContainer_KAV);
            this.Controls.Add(toolStripActionsTool_KAV);
            this.Controls.Add(menuStripControlMain_KAV);
            this.Controls.Add(statusStripAppState_KAV);
        }

        private NumericUpDown CreateNumericInput_KAV(string label, ref int y, decimal min, decimal max, int decPlaces = 0)
        {
            var lbl = new Label { Text = label, Left = 15, Top = y, Width = 280 };
            y += 20;
            var num = new NumericUpDown { Left = 15, Top = y, Width = 280, Minimum = min, Maximum = max, DecimalPlaces = decPlaces };
            groupBoxEditForm_KAV.Controls.Add(lbl);
            groupBoxEditForm_KAV.Controls.Add(num);
            y += 28;
            return num;
        }

        private void LoadInitialData_KAV()
        {
            if (File.Exists(_activeFilePath))
            {
                _apartments = new BindingList<ApartmentRecord>(_dataService.LoadFromFile(_activeFilePath));
            }
            else
            {
                _apartments = new BindingList<ApartmentRecord>
                {
                    new ApartmentRecord { EntranceNumber = 1, ApartmentNumber = 1, TotalArea = 42.5, LivingArea = 24.0, RoomsCount = 1, OwnerLastName = "Смирнов", RegistrationDate = DateTime.Now.AddYears(-3), FamilyMembersCount = 2, ChildrenCount = 0, HasDebt = false, Note = "Льготный тариф" },
                    new ApartmentRecord { EntranceNumber = 1, ApartmentNumber = 2, TotalArea = 64.2, LivingArea = 40.5, RoomsCount = 2, OwnerLastName = "Кузнецов", RegistrationDate = DateTime.Now.AddYears(-5), FamilyMembersCount = 3, ChildrenCount = 1, HasDebt = true, Note = "Задолженность 4500 руб" },
                    new ApartmentRecord { EntranceNumber = 2, ApartmentNumber = 15, TotalArea = 82.0, LivingArea = 55.0, RoomsCount = 3, OwnerLastName = "Васильев", RegistrationDate = DateTime.Now.AddYears(-1), FamilyMembersCount = 4, ChildrenCount = 2, HasDebt = false, Note = "Новые жильцы" },
                    new ApartmentRecord { EntranceNumber = 2, ApartmentNumber = 16, TotalArea = 58.4, LivingArea = 35.0, RoomsCount = 2, OwnerLastName = "Попов", RegistrationDate = DateTime.Now.AddYears(-8), FamilyMembersCount = 1, ChildrenCount = 0, HasDebt = false, Note = "Пенсионер" }
                };
                _dataService.SaveToFile(_activeFilePath, _apartments);
            }
            RefreshTableAndStats_KAV();
        }

        private void RefreshTableAndStats_KAV()
        {
            IEnumerable<ApartmentRecord> filtered = _apartments;

            string search = textBoxSearchByOwner_KAV.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(x => (x.OwnerLastName ?? "").ToLower().Contains(search) || (x.Note ?? "").ToLower().Contains(search));
            }

            if (comboBoxFilterEntrance_KAV.SelectedIndex > 0)
            {
                int entranceNum = comboBoxFilterEntrance_KAV.SelectedIndex;
                filtered = filtered.Where(x => x.EntranceNumber == entranceNum);
            }

            if (comboBoxFilterDebt_KAV.SelectedIndex == 1) filtered = filtered.Where(x => x.HasDebt);
            else if (comboBoxFilterDebt_KAV.SelectedIndex == 2) filtered = filtered.Where(x => !x.HasDebt);

            switch (comboBoxSortParameters_KAV.SelectedIndex)
            {
                case 0: filtered = filtered.OrderBy(x => x.ApartmentNumber); break;
                case 1: filtered = filtered.OrderBy(x => x.OwnerLastName); break;
                case 2: filtered = filtered.OrderBy(x => x.TotalArea); break;
                case 3: filtered = filtered.OrderByDescending(x => x.TotalArea); break;
            }

            var viewList = filtered.ToList();
            dataGridViewApartmentsList_KAV.DataSource = viewList;

            if (dataGridViewApartmentsList_KAV.Columns.Count > 0)
            {
                dataGridViewApartmentsList_KAV.Columns["EntranceNumber"].HeaderText = "Подъезд";
                dataGridViewApartmentsList_KAV.Columns["ApartmentNumber"].HeaderText = "Квартира";
                dataGridViewApartmentsList_KAV.Columns["TotalArea"].HeaderText = "Общ. пл. (м²)";
                dataGridViewApartmentsList_KAV.Columns["LivingArea"].HeaderText = "Жил. пл. (м²)";
                dataGridViewApartmentsList_KAV.Columns["RoomsCount"].HeaderText = "Комнат";
                dataGridViewApartmentsList_KAV.Columns["OwnerLastName"].HeaderText = "Фамилия";
                dataGridViewApartmentsList_KAV.Columns["RegistrationDate"].HeaderText = "Дата прописки";
                dataGridViewApartmentsList_KAV.Columns["FamilyMembersCount"].HeaderText = "Членов семьи";
                dataGridViewApartmentsList_KAV.Columns["ChildrenCount"].HeaderText = "Детей";
                dataGridViewApartmentsList_KAV.Columns["HasDebt"].HeaderText = "Долг";
                dataGridViewApartmentsList_KAV.Columns["Note"].HeaderText = "Примечание";
            }

            labelStatTotalCount_KAV.Text = $"Квартир в выборке: {_dataService.GetTotalApartments(viewList)}";
            labelStatTotalArea_KAV.Text = $"Суммарная площадь: {_dataService.GetSumTotalArea(viewList)} м²";
            labelStatAvgArea_KAV.Text = $"Средняя площадь: {_dataService.GetAverageTotalArea(viewList)} м²";
            labelStatMinMaxArea_KAV.Text = $"Мин/Макс: {_dataService.GetMinTotalArea(viewList)} / {_dataService.GetMaxTotalArea(viewList)} м²";
            labelStatDebtors_KAV.Text = $"Квартир с долгом: {_dataService.GetDebtorsCount(viewList)}";

            statusLabelDisplayCounter_KAV.Text = $"Отображено: {viewList.Count} из {_apartments.Count} | Всего жильцов: {_dataService.GetTotalResidents(viewList)}";

            UpdateChart_KAV(viewList);
        }

        private void UpdateChart_KAV(List<ApartmentRecord> records)
        {
            var series = chartDisplayDistribution_KAV.Series["Площадь квартир"];
            series.Points.Clear();

            foreach (var item in records.OrderBy(x => x.ApartmentNumber))
            {
                var point = series.Points.AddXY($"Кв. {item.ApartmentNumber}", item.TotalArea);
                if (item.HasDebt)
                {
                    series.Points[point].Color = Color.Salmon;
                }
            }
        }

        private void AddApartment_KAV()
        {
            if (string.IsNullOrWhiteSpace(textBoxInputOwnerLastName_KAV.Text))
            {
                MessageBox.Show("Пожалуйста, укажите фамилию квартиросъемщика!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int aptNum = (int)numericUpDownInputApartment_KAV.Value;
            if (_apartments.Any(x => x.ApartmentNumber == aptNum))
            {
                MessageBox.Show($"Квартира №{aptNum} уже зарегистрирована в системе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var record = new ApartmentRecord
            {
                EntranceNumber = (int)numericUpDownInputEntrance_KAV.Value,
                ApartmentNumber = aptNum,
                TotalArea = (double)numericUpDownInputTotalArea_KAV.Value,
                LivingArea = (double)numericUpDownInputLivingArea_KAV.Value,
                RoomsCount = (int)numericUpDownInputRooms_KAV.Value,
                OwnerLastName = textBoxInputOwnerLastName_KAV.Text.Trim(),
                RegistrationDate = dateTimePickerInputRegDate_KAV.Value,
                FamilyMembersCount = (int)numericUpDownInputFamilyCount_KAV.Value,
                ChildrenCount = (int)numericUpDownInputChildrenCount_KAV.Value,
                HasDebt = checkBoxInputHasDebt_KAV.Checked,
                Note = textBoxInputNote_KAV.Text.Trim()
            };

            _apartments.Add(record);
            _dataService.SaveToFile(_activeFilePath, _apartments);
            RefreshTableAndStats_KAV();
            ClearInputs_KAV();
        }

        private void UpdateApartment_KAV()
        {
            if (dataGridViewApartmentsList_KAV.CurrentRow?.DataBoundItem is ApartmentRecord selected)
            {
                var record = _apartments.FirstOrDefault(x => x.ApartmentNumber == selected.ApartmentNumber);
                if (record != null)
                {
                    record.EntranceNumber = (int)numericUpDownInputEntrance_KAV.Value;
                    record.TotalArea = (double)numericUpDownInputTotalArea_KAV.Value;
                    record.LivingArea = (double)numericUpDownInputLivingArea_KAV.Value;
                    record.RoomsCount = (int)numericUpDownInputRooms_KAV.Value;
                    record.OwnerLastName = textBoxInputOwnerLastName_KAV.Text.Trim();
                    record.RegistrationDate = dateTimePickerInputRegDate_KAV.Value;
                    record.FamilyMembersCount = (int)numericUpDownInputFamilyCount_KAV.Value;
                    record.ChildrenCount = (int)numericUpDownInputChildrenCount_KAV.Value;
                    record.HasDebt = checkBoxInputHasDebt_KAV.Checked;
                    record.Note = textBoxInputNote_KAV.Text.Trim();

                    _dataService.SaveToFile(_activeFilePath, _apartments);
                    RefreshTableAndStats_KAV();
                }
            }
        }

        private void DeleteApartment_KAV()
        {
            if (dataGridViewApartmentsList_KAV.CurrentRow?.DataBoundItem is ApartmentRecord selected)
            {
                var ask = MessageBox.Show($"Удалить сведения о квартире №{selected.ApartmentNumber} ({selected.OwnerLastName})?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    _apartments.Remove(selected);
                    _dataService.SaveToFile(_activeFilePath, _apartments);
                    RefreshTableAndStats_KAV();
                    ClearInputs_KAV();
                }
            }
        }

        private void OnGridSelectionChanged_KAV(object sender, EventArgs e)
        {
            if (dataGridViewApartmentsList_KAV.CurrentRow?.DataBoundItem is ApartmentRecord r)
            {
                numericUpDownInputEntrance_KAV.Value = r.EntranceNumber;
                numericUpDownInputApartment_KAV.Value = r.ApartmentNumber;
                numericUpDownInputTotalArea_KAV.Value = (decimal)r.TotalArea;
                numericUpDownInputLivingArea_KAV.Value = (decimal)r.LivingArea;
                numericUpDownInputRooms_KAV.Value = r.RoomsCount;
                textBoxInputOwnerLastName_KAV.Text = r.OwnerLastName;
                dateTimePickerInputRegDate_KAV.Value = r.RegistrationDate;
                numericUpDownInputFamilyCount_KAV.Value = r.FamilyMembersCount;
                numericUpDownInputChildrenCount_KAV.Value = r.ChildrenCount;
                checkBoxInputHasDebt_KAV.Checked = r.HasDebt;
                textBoxInputNote_KAV.Text = r.Note;
            }
        }

        private void ClearInputs_KAV()
        {
            numericUpDownInputEntrance_KAV.Value = 1;
            numericUpDownInputApartment_KAV.Value = 1;
            numericUpDownInputTotalArea_KAV.Value = 40;
            numericUpDownInputLivingArea_KAV.Value = 25;
            numericUpDownInputRooms_KAV.Value = 1;
            textBoxInputOwnerLastName_KAV.Clear();
            dateTimePickerInputRegDate_KAV.Value = DateTime.Now;
            numericUpDownInputFamilyCount_KAV.Value = 1;
            numericUpDownInputChildrenCount_KAV.Value = 0;
            checkBoxInputHasDebt_KAV.Checked = false;
            textBoxInputNote_KAV.Clear();
            dataGridViewApartmentsList_KAV.ClearSelection();
        }

        private void ResetFilters_KAV()
        {
            textBoxSearchByOwner_KAV.Clear();
            comboBoxFilterEntrance_KAV.SelectedIndex = 0;
            comboBoxFilterDebt_KAV.SelectedIndex = 0;
            comboBoxSortParameters_KAV.SelectedIndex = 0;
            RefreshTableAndStats_KAV();
        }

        private void ActionOpenFile_KAV()
        {
            using (var ofd = new OpenFileDialog { Filter = "Файлы CSV (*.csv)|*.csv|Все файлы (*.*)|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _activeFilePath = ofd.FileName;
                    var loaded = _dataService.LoadFromFile(_activeFilePath);
                    _apartments = new BindingList<ApartmentRecord>(loaded);
                    RefreshTableAndStats_KAV();
                }
            }
        }

        private void ActionSaveFile_KAV()
        {
            using (var sfd = new SaveFileDialog { Filter = "Файлы CSV (*.csv)|*.csv", FileName = _activeFilePath })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _activeFilePath = sfd.FileName;
                    _dataService.SaveToFile(_activeFilePath, _apartments);
                    MessageBox.Show("Файл успешно сохранен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormMain_KAV
            // 
            this.ClientSize = new System.Drawing.Size(1467, 772);
            this.Name = "FormMain_KAV";
            this.ResumeLayout(false);

        }
    }
}
