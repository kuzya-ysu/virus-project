﻿using System.Windows;

namespace VirusApp
{
    public partial class Inform : Window
    {
        public Inform()
        {
            InitializeComponent();
            InfText.Text = "С помощью данной программы вы сможете создать компьютерную модель распространения вирусного заболевания.\r\nРаспространение вируса в городе зависит от нескольких факторов: численности населения рассматриваемого города, насыщенности в нем транспортного сообщения, количества заболевших людей в городе, количества населения, у которого сделаны профилактические прививки и сезона года (заболеваемость растет с сентября, а с марта она уменьшается).\r\nЦель моделирования – выявление стратегий проведения прививок, которые позволяют минимизировать число заболевших вирусным заболеванием и избежать эпидемии в крупных городах. Шаг моделирования-одна неделя. На каждом шаге вы принимаете решение, в каких городах и в каком количестве сделать профилактические прививки (их количество должно допускаться текущим состоянием денежного фонда). Также на каждом шаге вы увидите, как изменилось количество заболевших в кадом городе и текущее состояние денежного фонда страны -при этом учитывается его убыль от сделанных (на этом шаге моделирования) прививок и выплат по временной нетрудоспособности для болеющего населения, а также прирост фонда за счет налоговых отчислений здоровой и работающей части населения городов. \r\nВ любой момент времени вы можете сохранить симуляцию, и продолжить работу с ней позже.";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }
    }
}