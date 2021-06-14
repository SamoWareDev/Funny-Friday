using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FunnyFriday
{
    class PlayState : GameState
    {
        #region Vars
        RenderWindow wnd;
        int gameTick = 0;
        int nWeek = 0;
        int nDifficulty = 0;
        float scrollSpeed = 1.5f;
        int score = 0;
        int misses = 0;

        Clock animationTime;
        Clock musicTime;

        List<Texture> enemyIdle;
        List<Texture> enemyLeft;
        List<Texture> enemyDown;
        List<Texture> enemyUp;
        List<Texture> enemyRight;

        List<Texture> playerIdle;
        List<Texture> playerLeft;
        List<Texture> playerDown;
        List<Texture> playerUp;
        List<Texture> playerRight;

        List<Texture> arrowLeft;
        List<Texture> arrowDown;
        List<Texture> arrowUp;
        List<Texture> arrowRight;

        List<Texture> noteArray;

        Dictionary<string, Sprite> spriteContainer = new Dictionary<string, Sprite>();
        List<Sprite> arrowContainer = new List<Sprite>();
        List<Sprite> enemyArrowContainer = new List<Sprite>();

        List<List<Sprite>> enemyLane = new List<List<Sprite>>();
        List<List<Sprite>> playerLane = new List<List<Sprite>>();

        List<Texture> ratingTextures = new List<Texture>();
        List<Sprite> ratingContainer = new List<Sprite>();


        bool bLeft = false;
        bool bDown = false;
        bool bUp = false;
        bool bRight = false;

        Song currentSong;

        Sound instrumental;
        Sound voice;

        [DllImport("User32.dll")]
        public static extern bool GetAsyncKeyState(int key);

        Font regularFont;
        Text scoreText;
        Text missText;

        #endregion

        public PlayState(RenderWindow _wnd, int week, int difficulty)
        {
            wnd = _wnd;
            nWeek = week;
            nDifficulty = difficulty;

            regularFont = new Font("Assets/Fonts/vcr.ttf");
            scoreText = new Text("Score: " + score, regularFont);
            scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

            missText = new Text("Miss: " + score, regularFont);
            missText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, missText.GetLocalBounds().Height);


            var textureManager = new TextureManager("Assets/XML/Notes.xml");

            arrowLeft = textureManager.GetTextures(new string[] { "arrowLEFT0000", "left press0003", "left confirm0003" });
            arrowDown = textureManager.GetTextures(new string[] { "arrowDOWN0000", "down press0003", "down confirm0003" });
            arrowUp = textureManager.GetTextures(new string[] { "arrowUP0000", "up press0003", "up confirm0003" });
            arrowRight = textureManager.GetTextures(new string[] { "arrowRIGHT0000", "right press0003", "right confirm0003" });

            noteArray = textureManager.GetTextures(new string[] { "left confirm0003", "down confirm0003", "up confirm0003", "right confirm0003" });

            textureManager = new TextureManager("Assets/XML/Boyfriend.xml");

            playerIdle = textureManager.GetTextures(430, 443);
            playerLeft = textureManager.GetTextures(157, 171);
            playerDown = textureManager.GetTextures(98, 127);
            playerUp = textureManager.GetTextures(314, 328);
            playerRight = textureManager.GetTextures(206, 267);

            spriteContainer.Add("player", new Sprite(playerIdle[0]));
            spriteContainer["player"].Scale = new Vector2f(0.5f, 0.5f);
            spriteContainer["player"].Position = new Vector2f(850, 400);

            arrowContainer.Add(new Sprite(arrowLeft[0]));
            arrowContainer.Add(new Sprite(arrowDown[0]));
            arrowContainer.Add(new Sprite(arrowUp[0]));
            arrowContainer.Add(new Sprite(arrowRight[0]));

            enemyArrowContainer.Add(new Sprite(arrowLeft[0]));
            enemyArrowContainer.Add(new Sprite(arrowDown[0]));
            enemyArrowContainer.Add(new Sprite(arrowUp[0]));
            enemyArrowContainer.Add(new Sprite(arrowRight[0]));

            for (int i = 0; i < 4; i++)
            {
                arrowContainer[i].Scale = new Vector2f(0.7f, 0.7f);
                enemyArrowContainer[i].Scale = new Vector2f(0.7f, 0.7f);
                arrowContainer[i].Position = new Vector2f(750 + 125 * i, 50);
                enemyArrowContainer[i].Position = new Vector2f(50 + 125 * i, 50);
            }


            for (int i = 0; i < 4; i++)
            {
                enemyLane.Add(new List<Sprite>());
                playerLane.Add(new List<Sprite>());
            }

            var chart = new ChartParser("Assets/Music/Philly/blammed");
            currentSong = chart.GetSong();

            for (int i = 0; i < currentSong.song.notes.Length; i++)
                for (int j = 0; j < currentSong.song.notes[i].sectionNotes.Count; j++)
                {
                    double yPos = currentSong.song.notes[i].sectionNotes[j][0];
                    int lane = Convert.ToInt32(currentSong.song.notes[i].sectionNotes[j][1]);
                    double length = currentSong.song.notes[i].sectionNotes[j][2];
                    try
                    {
                        if (currentSong.song.notes[i].mustHitSection)
                        {
                            playerLane[lane].Add(new Sprite(noteArray[lane]));
                            playerLane[lane][playerLane[lane].Count - 1].Position = new Vector2f(arrowContainer[lane].Position.X - 20, (float)yPos * scrollSpeed);
                            playerLane[lane][playerLane[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                        }
                        else
                        {
                            enemyLane[lane].Add(new Sprite(noteArray[lane]));
                            enemyLane[lane][enemyLane[lane].Count - 1].Position = new Vector2f(enemyArrowContainer[lane].Position.X - 20, (float)yPos * scrollSpeed);
                            enemyLane[lane][enemyLane[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                        }
                    }
                    catch { }
                }

            //ratingTextures.Add(new Texture("Assets/Ratings/sick.png"));
            //ratingTextures.Add(new Texture("Assets/Ratings/good.png"));
            //ratingTextures.Add(new Texture("Assets/Ratings/bad.png"));

            instrumental = new Sound(new SoundBuffer("Assets/Music/Philly/Inst.ogg"));
            voice = new Sound(new SoundBuffer("Assets/Music/Philly/Voices.ogg"));

            voice.Play();
            instrumental.Play();

            animationTime = new Clock();
            musicTime = new Clock();
        }

        public override void Draw()
        {
            foreach (var s in spriteContainer)
                wnd.Draw(s.Value);

            foreach (var s in arrowContainer)
                wnd.Draw(s);

            foreach (var s in enemyArrowContainer)
                wnd.Draw(s);

            foreach (var s in enemyLane)
                foreach (var d in s)
                    wnd.Draw(d);

            foreach (var s in playerLane)
                foreach (var d in s)
                    wnd.Draw(d);

            foreach (var s in ratingContainer)
                wnd.Draw(s);

            wnd.Draw(scoreText);
            wnd.Draw(missText);
        }

        public override void Update(ref Clock deltaTime)
        {
            float delta = musicTime.ElapsedTime.AsSeconds() * 1000.0f * (float)scrollSpeed;
            if (delta <= 6.5f * scrollSpeed)
                delta = 6.5f * scrollSpeed;

            try
            {
                for (int i = 0; i < playerLane.Count; i++)
                    for (int j = 0; j < playerLane[i].Count; j++)
                    {
                        playerLane[i][j].Position = new Vector2f(playerLane[i][j].Position.X, playerLane[i][j].Position.Y - delta);

                        if (playerLane[i][j].Position.Y <= arrowContainer[i].Position.Y - 175.0f)
                        {
                            playerLane[i].Remove(playerLane[i][j]);
                            misses++;
                            missText.DisplayedString = "Miss: " + misses;
                            missText.Position = new Vector2f(1280 / 2 - missText.GetLocalBounds().Width / 2, missText.GetLocalBounds().Height);
                        }
                    }


                for (int i = 0; i < enemyLane.Count; i++)
                    for (int j = 0; j < enemyLane[i].Count; j++)
                    {
                        if (enemyLane[i][j].Position.Y <= enemyArrowContainer[i].Position.Y)
                            enemyLane[i].Remove(enemyLane[i][j]);

                        enemyLane[i][j].Position = new Vector2f(enemyLane[i][j].Position.X, enemyLane[i][j].Position.Y - delta);
                    }

                musicTime.Restart();
            }
            catch { }
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            //try
            //{
            //    if (ratingContainer.Count > 0 && animationTime.ElapsedTime.AsSeconds() >= 0.04f)
            //    {
            //        foreach (var s in ratingContainer)
            //        {
            //            s.Position = new Vector2f(s.Position.X, s.Position.Y - 2);

            //            if (s.Position.Y < 720 / 2 - 25)
            //                ratingContainer.Remove(s);
            //        }
            //        animationTime.Restart();
            //    }
            //}
            //catch { }

            if (animationTime.ElapsedTime.AsSeconds() >= 0.04f)
            {
                spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)playerIdle[gameTick % playerIdle.Count].Size.X, (int)playerIdle[gameTick % playerIdle.Count].Size.Y);
                spriteContainer["player"].Texture = playerIdle[gameTick % playerIdle.Count];
                gameTick++;
                animationTime.Restart();
            }

            if (GetAsyncKeyState(0x59))
            {
                var activeArrow = arrowLeft[1];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;

                if (playerLane[0].Count > 0)
                    if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 120 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.15f / scrollSpeed)
                        {
                            if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                                //ratingContainer.Add(new Sprite(ratingTextures[0]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.3f, 0.3f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 60,
                                //    720 / 2);
                            }
                            else if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                                //ratingContainer.Add(new Sprite(ratingTextures[1]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.4f, 0.4f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 20,
                                //    720 / 2);
                            }
                            else if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 120 * scrollSpeed)
                            {
                                score += 25;
                                //ratingContainer.Add(new Sprite(ratingTextures[2]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.5f, 0.5f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2,
                                //    720 / 2);
                            }

                            playerLane[0].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        var activeTexture = playerLeft;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280  / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                bLeft = false;
                var activeArrow = arrowLeft[0];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;
            }

            if (GetAsyncKeyState(0x58))
            {
                var activeArrow = arrowDown[1];
                arrowContainer[1].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[1].Texture = activeArrow;

                if (playerLane[1].Count > 0)
                    if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 120 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.15f / scrollSpeed)
                        {
                            if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                                //ratingContainer.Add(new Sprite(ratingTextures[0]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.3f, 0.3f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 60,
                                //    720 / 2);
                            }
                            else if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                                //ratingContainer.Add(new Sprite(ratingTextures[1]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.4f, 0.4f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 20,
                                //    720 / 2);
                            }
                            else if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 120 * scrollSpeed)
                            {
                                score += 25;
                                //ratingContainer.Add(new Sprite(ratingTextures[2]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.5f, 0.5f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2,
                                //    720 / 2);
                            }

                            playerLane[1].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        var activeTexture = playerDown;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                        
                    }
            }
            else
            {
                var activeArrow = arrowDown[0];
                arrowContainer[1].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[1].Texture = activeArrow;
            }

            if (GetAsyncKeyState(0xBC))
            {
                var activeArrow = arrowUp[1];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;

                if (playerLane[2].Count > 0)
                    if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 120 * scrollSpeed )
                    {
                        if(deltaTime.ElapsedTime.AsSeconds() >= 0.15f / scrollSpeed)
                        {
                            if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                                //ratingContainer.Add(new Sprite(ratingTextures[0]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.3f, 0.3f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 60,
                                //    720 / 2);
                            }
                            else if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                                //ratingContainer.Add(new Sprite(ratingTextures[1]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.4f, 0.4f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 20,
                                //    720 / 2);
                            }
                            else if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 120 * scrollSpeed)
                            {
                                score += 25;
                                //ratingContainer.Add(new Sprite(ratingTextures[2]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.5f, 0.5f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2,
                                //    720 / 2);
                            }

                            playerLane[2].RemoveAt(0);
                            deltaTime.Restart();
                        }
                       

                        var activeTexture = playerUp;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                bUp = false;
                var activeArrow = arrowUp[0];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;
            }


            if (GetAsyncKeyState(0xBE))
            {
                var activeArrow = arrowRight[1];
                arrowContainer[3].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[3].Texture = activeArrow;

                if (playerLane[3].Count > 0)
                    if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 120 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.15f / scrollSpeed)
                        {
                            if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                                //ratingContainer.Add(new Sprite(ratingTextures[0]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.3f, 0.3f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 60,
                                //    720 / 2);
                            }
                            else if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                                //ratingContainer.Add(new Sprite(ratingTextures[1]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.4f, 0.4f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2 + 20,
                                //    720 / 2);
                            }
                            else if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 120 * scrollSpeed)
                            {
                                score += 25;
                                //ratingContainer.Add(new Sprite(ratingTextures[2]));
                                //ratingContainer[ratingContainer.Count - 1].Scale = new Vector2f(0.5f, 0.5f);
                                //ratingContainer[ratingContainer.Count - 1].Position = new Vector2f(1280 / 2 - ratingContainer[ratingContainer.Count - 1].GetLocalBounds().Width / 2,
                                //    720 / 2);
                            }

                            playerLane[3].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                bRight = false;
                var activeArrow = arrowRight[0];
                arrowContainer[3].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[3].Texture = activeArrow;
            }
        }
    }
}
