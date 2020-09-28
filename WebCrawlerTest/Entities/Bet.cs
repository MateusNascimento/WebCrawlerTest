using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawlerTest.Entities
{
    public class Bet
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Champ { get; set; }
        public string Team { get; set; }
        public string CounterTeam { get; set; }
        public float Odd { get; set; }
        public override string ToString()
        {
            return $"[Type: {Type}, Detail: {Detail}, " +
                $"Team: {Team}, CounterTeam: {CounterTeam}, Odd: {Odd}]";
        }

    }
}
