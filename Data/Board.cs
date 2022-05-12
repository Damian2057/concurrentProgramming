﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Board
    {
        private readonly int _width;
        private readonly int _height;
        private bool _isMoving = false;
        private object _lock = new object();
        private readonly List<BallApi> _balls = new();
        private List<Thread> _threads;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _threads = new List<Thread>();
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        public void AddBall(BallApi ball)
        {
            _balls.Add(ball);

            Thread t = new Thread(() => {

                while (_isMoving)
                {
                    lock (_lock)
                    {
                        ball.MoveBall();

                        while (ball.IsMoving) { }
                    }
                    Thread.Sleep(2);

                }

            });

            _threads.Add(t);
        }

        public void StartAnimation()
        {
            _isMoving = true;
            foreach (Thread t in _threads)
            {
                t.Start();
            }

        }

        public void StopAnimation()
        {
            _isMoving = false;
        }

        public List<BallApi> GetBalls()
        {
            return _balls;
        }

        public void ClearBalls()
        {
            _balls.Clear();
            _threads.Clear();
        }
    }
}
