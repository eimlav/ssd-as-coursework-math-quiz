using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_
{
    public class Question
    {
        private int Time;
        private int Score;
        private int tempScore;        

        // constructor 
        public Question()
        {
            Time = 150;
            Score = 0;
        }

        // returns tempScore
        public int GetTempScore()
        {
            return tempScore;
        }

        // returns Time
        public int GetTime()
        {
            return Time;
        }

        // sets Time to the passed in int value
        public void SetTime(int time)
        {
            Time = time;
        }

        // sets Score to the passed in int value
        public void SetScore(int score)
        {
            Score = Score + score;
        }

        // increases Score by 1
        public int IncreaseScore()
        {
            tempScore++;
            return tempScore;
        }
        
    }
}
