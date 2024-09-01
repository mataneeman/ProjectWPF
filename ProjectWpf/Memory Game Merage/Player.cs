using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWpf.Memory_Game_Merage
{
    public class Player
    {
        public string? Name { get; set; }
        public int Score { get; set; }

        public void IncreaseScore()
        {
            Score++;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }


}
