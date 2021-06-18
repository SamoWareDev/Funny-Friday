using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace FunnyFriday
{
    class PlayState : GameState
    {
        #region Vars
        RenderWindow wnd;
        int gameTick = 0;
        float scrollSpeed = 1.5f;
        int score = 0;
        int misses = 0;

        int nWeek = 0;
        int nDifficulty = 0;
        int nDay = 0;

        Clock animationTime;
        Clock animationTime2;
        Clock musicTime;

        bool bNext = false;

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


        int currPlayerPos = -1;
        int currEnemyPos = -1;

        ChartParser chart;
              
        Song currentSong;

        Sound instrumental;
        Sound voice;

        Font regularFont;
        Text scoreText;
        Text missText;

        float delta = 0.0f;

        #endregion

        public PlayState(RenderWindow _wnd, int week, int day, int difficulty)
        {
            wnd = _wnd;
            nWeek = week;
            nDay = day;
            nDifficulty = difficulty;

            regularFont = new Font("Assets/Fonts/vcr.ttf");
            scoreText = new Text("Score: " + score, regularFont);
            scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

            missText = new Text("Miss: " + score, regularFont);
            missText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, missText.GetLocalBounds().Height);

            spriteContainer.Add("backcock", new Sprite(new Texture("Assets/red.png")));
            spriteContainer.Add("background", new Sprite(new Texture("Assets/island.png")));
            spriteContainer["background"].Scale = new Vector2f(0.65f, 0.65f);
            spriteContainer["background"].Position = new Vector2f(-600.0f, -200.0f);

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
            spriteContainer["player"].Position = new Vector2f(900, 400);

            switch(week)
            {
                case 0:
                    {
                        switch (day)
                        {
                            case 0:
                                {
                                    switch (difficulty)
                                    {
                                        case 0:
                                            chart = new ChartParser("Assets/Music/Improbable Outset/improbable-outset-easy");
                                            break;
                                        case 1:
                                            chart = new ChartParser("Assets/Music/Improbable Outset/improbable-outset");
                                            break;
                                        case 2:
                                            chart = new ChartParser("Assets/Music/Improbable Outset/improbable-outset-hard");
                                            break;
                                    }
                                    instrumental = new Sound(new SoundBuffer("Assets/Music/Improbable Outset/Inst.ogg"));
                                    voice = new Sound(new SoundBuffer("Assets/Music/Improbable Outset/Voices.ogg"));

                                    textureManager = new TextureManager("Assets/XML/trickyMask.xml");

                                    enemyIdle = textureManager.GetTextures(4, 28);
                                    enemyLeft = textureManager.GetTextures(51, 60);
                                    enemyDown = textureManager.GetTextures(30, 35);
                                    enemyUp = textureManager.GetTextures(95, 103);
                                    enemyRight = textureManager.GetTextures(73, 80);

                                    spriteContainer.Add("enemy", new Sprite(enemyIdle[0]));
                                    spriteContainer["enemy"].Scale = new Vector2f(0.4f, 0.4f);
                                    spriteContainer["enemy"].Position = new Vector2f(150, 300);
                                    bNext = true;
                                }
                                break;
                            case 1:
                                {
                                    switch (difficulty)
                                    {
                                        case 0:
                                            chart = new ChartParser("Assets/Music/Madness/madness-easy");
                                            break;
                                        case 1:
                                            chart = new ChartParser("Assets/Music/Madness/madness");
                                            break;
                                        case 2:
                                            chart = new ChartParser("Assets/Music/Madness/madness-hard");
                                            break;
                                    }

                                    instrumental = new Sound(new SoundBuffer("Assets/Music/Madness/Inst.ogg"));
                                    voice = new Sound(new SoundBuffer("Assets/Music/Madness/Voices.ogg"));

                                    textureManager = new TextureManager("Assets/XML/tricky.xml");

                                    enemyIdle = textureManager.GetTextures(4, 29);
                                    enemyLeft = textureManager.GetTextures(67, 70);
                                    enemyDown = textureManager.GetTextures(30, 35);
                                    enemyUp = textureManager.GetTextures(132, 137);
                                    enemyRight = textureManager.GetTextures(104, 110);

                                    spriteContainer.Add("enemy", new Sprite(enemyIdle[0]));
                                    spriteContainer["enemy"].Scale = new Vector2f(0.4f, 0.4f);
                                    spriteContainer["enemy"].Position = new Vector2f(150, 300);
                                }
                                break;
                        }
                       

                        currentSong = chart.GetSong();
                    }
                    break;
            }

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
                            playerLane[lane][playerLane[lane].Count - 1].Position = new Vector2f(arrowContainer[lane].Position.X - 20, (float)(yPos - 4 * i)* scrollSpeed);
                            playerLane[lane][playerLane[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                        }
                        else
                        {
                            enemyLane[lane].Add(new Sprite(noteArray[lane]));
                            enemyLane[lane][enemyLane[lane].Count - 1].Position = new Vector2f(enemyArrowContainer[lane].Position.X - 20, (float)(yPos - 4 * i) * scrollSpeed);
                            enemyLane[lane][enemyLane[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                        }
                    }
                    catch { }
                }


            voice.Play();
            instrumental.Play();

            animationTime = new Clock();
            animationTime2 = new Clock();
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

            wnd.Draw(scoreText);
            wnd.Draw(missText);
        }

        public override void Update(ref Clock deltaTime)
        {
            delta = musicTime.ElapsedTime.AsSeconds() * 1000.0f * (float)scrollSpeed;

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
                        {
                            currEnemyPos = i;
                            enemyLane[i].Remove(enemyLane[i][j]);
                        }

                        enemyLane[i][j].Position = new Vector2f(enemyLane[i][j].Position.X, enemyLane[i][j].Position.Y - delta);
                    }
            }
            catch { }

            if (animationTime.ElapsedTime.AsSeconds() >= 0.04f)
            {
                switch (currPlayerPos)
                {
                    case -1:
                        {
                            spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)playerIdle[gameTick % playerIdle.Count].Size.X, (int)playerIdle[gameTick % playerIdle.Count].Size.Y);
                            spriteContainer["player"].Texture = playerIdle[gameTick % playerIdle.Count];
                        }
                        break;

                    case 0:
                        {
                            var activeTexture = playerLeft;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["player"].Texture = activeTexture[index];
                        }
                        break;

                    case 1:
                        {
                            var activeTexture = playerDown;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["player"].Texture = activeTexture[index];
                        }
                        break;

                    case 2:
                        {
                            var activeTexture = playerUp;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["player"].Texture = activeTexture[index];
                        }
                        break;

                    case 3:
                        {
                            var activeTexture = playerRight;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["player"].Texture = activeTexture[index];
                        }
                        break;
                }

                switch (currEnemyPos)
                {
                    case -1:
                        {
                            spriteContainer["enemy"].TextureRect = new IntRect(0, 0, (int)enemyIdle[gameTick % enemyIdle.Count].Size.X, (int)enemyIdle[gameTick % enemyIdle.Count].Size.Y);
                            spriteContainer["enemy"].Texture = enemyIdle[gameTick % enemyIdle.Count];
                        }
                        break;
                    case 0:
                        {
                            spriteContainer["enemy"].TextureRect = new IntRect(0, 0, (int)enemyLeft[gameTick % enemyLeft.Count].Size.X, (int)enemyLeft[gameTick % enemyLeft.Count].Size.Y);
                            spriteContainer["enemy"].Texture = enemyLeft[gameTick % enemyLeft.Count];
                        }
                        break;

                    case 1:
                        {
                            var activeTexture = enemyDown;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["enemy"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["enemy"].Texture = activeTexture[index];
                        }
                        break;
                    case 2:
                        {
                            var activeTexture = enemyUp;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["enemy"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["enemy"].Texture = activeTexture[index];
                        }
                        break;
                    case 3:
                        {
                            var activeTexture = enemyRight;
                            var index = gameTick % activeTexture.Count;

                            spriteContainer["enemy"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                            spriteContainer["enemy"].Texture = activeTexture[index];
                        }
                        break;
                }

                gameTick++;
                animationTime.Restart();

                if (animationTime2.ElapsedTime.AsSeconds() >= 0.3f)
                {
                    if (currPlayerPos != -1)
                        currPlayerPos = -1;

                    if (currEnemyPos != -1)
                        currEnemyPos = -1;

                    animationTime2.Restart();
                }
            }

            musicTime.Restart();
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Y))
            {
                var activeArrow = arrowLeft[1];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;

                if (playerLane[0].Count > 0)
                    if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 150 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.10f)
                        {
                            if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 40 * scrollSpeed)
                                score += 100;
                            else if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 80 * scrollSpeed)
                                score += 50;
                            else if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 150 * scrollSpeed)
                                score += 25;


                            playerLane[0].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        currPlayerPos = 0;

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                var activeArrow = arrowLeft[0];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.X))
            {
                var activeArrow = arrowDown[1];
                arrowContainer[1].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[1].Texture = activeArrow;

                if (playerLane[1].Count > 0)
                    if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 150 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.10f)
                        {
                            if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                            }
                            else if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                            }
                            else if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 150 * scrollSpeed)
                            {
                                score += 25;
                            }

                            playerLane[1].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        currPlayerPos = 1;

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

            if (Keyboard.IsKeyPressed(Keyboard.Key.Comma))
            {
                var activeArrow = arrowUp[1];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;

                if (playerLane[2].Count > 0)
                    if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 150 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.10f)
                        {
                            if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                            }
                            else if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                            }
                            else if (playerLane[2][0].Position.Y <= arrowContainer[2].Position.Y + 150 * scrollSpeed)
                            {
                                score += 25;
                            }

                            playerLane[2].RemoveAt(0);
                            deltaTime.Restart();
                        }


                        currPlayerPos = 2;

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                var activeArrow = arrowUp[0];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.Period))
            {
                var activeArrow = arrowRight[1];
                arrowContainer[3].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[3].Texture = activeArrow;

                if (playerLane[3].Count > 0)
                    if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 150 * scrollSpeed)
                    {
                        if (deltaTime.ElapsedTime.AsSeconds() >= 0.10f)
                        {
                            if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 40 * scrollSpeed)
                            {
                                score += 100;
                            }
                            else if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 80 * scrollSpeed)
                            {
                                score += 50;
                            }
                            else if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 150 * scrollSpeed)
                            {
                                score += 25;
                            }

                            playerLane[3].RemoveAt(0);
                            deltaTime.Restart();
                        }

                        currPlayerPos = 3;

                        scoreText.DisplayedString = "Score: " + score;
                        scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

                        gameTick++;
                    }
            }
            else
            {
                var activeArrow = arrowRight[0];
                arrowContainer[3].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[3].Texture = activeArrow;
            }

            if(instrumental.Status == 0 && bNext)
                stack.ChangeStack(new PlayState(wnd, nWeek, nDay + 1, nDifficulty));

            if (instrumental.Status == 0 && bNext == false && nDay >= 1)
                stack.ChangeStack(new SelectScreen(wnd));
        }
    }
}
