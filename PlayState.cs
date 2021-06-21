using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

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

        List<List<Sprite>> lanes = new List<List<Sprite>>();

        bool[] bPressed = { false, false, false, false };
        

        int currPlayerPos = -1;
        int currEnemyPos = -1;

        bool bFocusPlayer = false;
        bool bFocusEnemy = true;
        Vector2f vPlayer;
        Vector2f vEnemy;

        ChartParser chart;
              
        Song currentSong;

        Sound instrumental;
        Sound voice;

        Font regularFont;
        Text scoreText;
        Text missText;

        float delta = 0.0f;

        View view;

        Keyboard.Key[] keys = new Keyboard.Key[4];

        #endregion

        public PlayState(RenderWindow _wnd, int week, int day, int difficulty)
        {
            wnd = _wnd;
            nWeek = week;
            nDay = day;
            nDifficulty = difficulty;

            string[] options = File.ReadAllLines("Assets/Options.txt");

            for (int i = 0; i < 4; i++)
                keys[i] = (Keyboard.Key)Convert.ToInt32(options[i]);

            scrollSpeed = (float)Convert.ToDouble(options[4]);

            menuMusic.Stop();

            view = new View(new FloatRect(0, 0, 1280, 720));
            view.Zoom(0.9f);

            regularFont = new Font("Assets/Fonts/vcr.ttf");
            scoreText = new Text("Score: " + score, regularFont);
            scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, 0);

            missText = new Text("Miss: " + score, regularFont);
            missText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width / 2, missText.GetLocalBounds().Height);

            spriteContainer.Add("red", new Sprite(new Texture("Assets/Tricky/red.png")));
            spriteContainer["red"].Position = new Vector2f(-500, -500);
            

            var textureManager = new TextureManager("Assets/XML/Notes.xml");

            arrowLeft = textureManager.GetTextures(new string[] { "arrowLEFT0000", "left press0003", "left confirm0003" });
            arrowDown = textureManager.GetTextures(new string[] { "arrowDOWN0000", "down press0003", "down confirm0003" });
            arrowUp = textureManager.GetTextures(new string[] { "arrowUP0000", "up press0003", "up confirm0003" });
            arrowRight = textureManager.GetTextures(new string[] { "arrowRIGHT0000", "right press0003", "right confirm0003" });

            noteArray = textureManager.GetTextures(new string[] { "left confirm0003", "down confirm0003", "up confirm0003", "right confirm0003" });

            switch(week)
            {
                case 0:
                    {
                        switch (day)
                        {
                            case 0:
                                {
                                    spriteContainer.Add("background", new Sprite(new Texture("Assets/Tricky/island.png")));
                                    spriteContainer["background"].Scale = new Vector2f(0.65f, 0.65f);
                                    spriteContainer["background"].Position = new Vector2f(-600.0f, -200.0f);

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
                                    spriteContainer.Add("background", new Sprite(new Texture("Assets/Tricky/island.png")));
                                    spriteContainer["background"].Scale = new Vector2f(0.65f, 0.65f);
                                    spriteContainer["background"].Position = new Vector2f(-600.0f, -200.0f);

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

                                    bNext = true;
                                }
                                break;

                            case 2:
                                {
                                    spriteContainer.Add("background", new Sprite(new Texture("Assets/Tricky/island.png")));
                                    spriteContainer["background"].Scale = new Vector2f(0.65f, 0.65f);
                                    spriteContainer["background"].Position = new Vector2f(-600.0f, -200.0f);

                                    switch (difficulty)
                                    {
                                        case 0:
                                            chart = new ChartParser("Assets/Music/Hell Clown/hellclown-easy");
                                            break;
                                        case 1:
                                            chart = new ChartParser("Assets/Music/Hell Clown/hellclown");
                                            break;
                                        case 2:
                                            chart = new ChartParser("Assets/Music/Hell Clown/hellclown-hard");
                                            bNext = true;
                                            break;
                                    }

                                    instrumental = new Sound(new SoundBuffer("Assets/Music/Hell Clown/Inst.ogg"));
                                    voice = new Sound(new SoundBuffer("Assets/Music/Hell Clown/Voices.ogg"));

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

                            case 3:
                                {
                                    spriteContainer.Add("background", new Sprite(new Texture("Assets/Tricky/daBackground.png")));
                                    spriteContainer["background"].Scale = new Vector2f(0.8f, 0.8f);
                                    spriteContainer["background"].Position = new Vector2f(-500.0f, -250);
                                    

                                    chart = new ChartParser("Assets/Music/Expurgation/expurgation-hard");

                                    instrumental = new Sound(new SoundBuffer("Assets/Music/Expurgation/Inst.ogg"));
                                    voice = new Sound(new SoundBuffer("Assets/Music/Expurgation/Voices.ogg"));

                                    textureManager = new TextureManager("Assets/XML/EXCLOWN.xml");

                                    enemyIdle = textureManager.GetTextures(4, 38);
                                    enemyLeft = textureManager.GetTextures(48, 54);
                                    enemyDown = textureManager.GetTextures(39, 46);
                                    enemyUp = textureManager.GetTextures(63, 70);
                                    enemyRight = textureManager.GetTextures(55, 62);

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

            textureManager = new TextureManager("Assets/XML/Boyfriend.xml");

            playerIdle = textureManager.GetTextures(430, 443);
            playerLeft = textureManager.GetTextures(157, 161);
            playerDown = textureManager.GetTextures(98, 102);
            playerUp = textureManager.GetTextures(314, 318);
            playerRight = textureManager.GetTextures(206, 210);

            spriteContainer.Add("player", new Sprite(playerIdle[0]));
            spriteContainer["player"].Scale = new Vector2f(0.5f, 0.5f);
            spriteContainer["player"].Position = new Vector2f(900, 400);

            vPlayer = new Vector2f(spriteContainer["player"].Position.X, spriteContainer["player"].Position.Y);
            vEnemy = new Vector2f(spriteContainer["enemy"].Position.X + spriteContainer["enemy"].GetLocalBounds().Width / 2, spriteContainer["enemy"].Position.Y);

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

            for (int i = 0; i < 8; i++)
                lanes.Add(new List<Sprite>());

            for(int i = 0; i < currentSong.song.notes.Length; i++)
                for(int j = 0; j < currentSong.song.notes[i].sectionNotes.Count; j++)
                {
                    try
                    {
                        double yPos = currentSong.song.notes[i].sectionNotes[j][0];
                        int lane = Convert.ToInt32(currentSong.song.notes[i].sectionNotes[j][1]);

                        if(currentSong.song.notes[i].mustHitSection)
                        {
                            if (lane <= 3)
                            {
                                lanes[lane + 4].Add(new Sprite(noteArray[lane]));
                                lanes[lane + 4][lanes[lane + 4].Count - 1].Position = new Vector2f(0, (float)(yPos - 4 * i) * scrollSpeed);
                                lanes[lane + 4][lanes[lane + 4].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                            }
                            if (lane >= 4)
                            {
                                lanes[lane - 4].Add(new Sprite(noteArray[lane - 4]));
                                lanes[lane - 4][lanes[lane - 4].Count - 1].Position = new Vector2f(0, (float)(yPos - 4 * i) * scrollSpeed);
                                lanes[lane - 4][lanes[lane - 4].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                            }
                        }
                        else
                        {
                            if (lane <= 3)
                            {
                                lanes[lane].Add(new Sprite(noteArray[lane]));
                                lanes[lane][lanes[lane].Count - 1].Position = new Vector2f(0, (float)(yPos - 4 * i) * scrollSpeed);
                                lanes[lane][lanes[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                            }
                            if(lane >= 4)
                            {
                                lanes[lane].Add(new Sprite(noteArray[lane - 4]));
                                lanes[lane][lanes[lane].Count - 1].Position = new Vector2f(0, (float)(yPos - 4 * i) * scrollSpeed);
                                lanes[lane][lanes[lane].Count - 1].Scale = new Vector2f(0.7f, 0.7f);
                            }
                        } 
                    }
                    catch { }
                }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < lanes[i].Count; j++)
                {
                    lanes[i][j].Position = new Vector2f(enemyArrowContainer[i].Position.X - 20, lanes[i][j].Position.Y);
                    enemyLane[i].Add(lanes[i][j]);
                }


            for (int i = 4; i < 8; i++)
                for (int j = 0; j < lanes[i].Count; j++)
                {
                    lanes[i][j].Position = new Vector2f(arrowContainer[i - 4].Position.X - 20, lanes[i][j].Position.Y);
                    playerLane[i - 4].Add(lanes[i][j]);
                }

            voice.Play();
            instrumental.Play();

            animationTime = new Clock();
            animationTime2 = new Clock();
            musicTime = new Clock();
        }

        public override void Draw()
        {
            wnd.SetView(view);

            foreach (var s in spriteContainer)
                wnd.Draw(s.Value);

            wnd.SetView(wnd.DefaultView);

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

        public void LerpTo(Vector2f pos, ref bool b)
        {
            if (view.Center != pos)
                view.Center = new Vector2f(view.Center.X + (pos.X - view.Center.X) / 20, view.Center.Y + (pos.Y - view.Center.Y) / 20);

            if (Math.Sqrt(Math.Pow(pos.X - view.Center.X, 2) + Math.Pow(pos.Y - view.Center.Y, 2)) <= 0.01f)
                b = false;
        }

        public override void Update(ref Clock deltaTime)
        {
            delta = musicTime.ElapsedTime.AsSeconds() * 1000.0f * scrollSpeed;

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

                if (animationTime2.ElapsedTime.AsSeconds() >= 0.5f)
                {
                    if (currPlayerPos != -1)
                        currPlayerPos = -1;

                    if (currEnemyPos != -1)
                        currEnemyPos = -1;
                    animationTime2.Restart();
                }
            }

            if (bFocusPlayer && !bFocusEnemy)
                LerpTo(vPlayer, ref bFocusPlayer);

            if (bFocusEnemy && !bFocusPlayer)
                LerpTo(vEnemy, ref bFocusEnemy);

            if (bFocusPlayer && bFocusEnemy)
                bFocusEnemy = false;

            for (int i = 0; i < playerLane.Count; i++)
            {
                if (playerLane[i].Count > 0)
                    if (playerLane[i][0].Position.Y <= arrowContainer[i].Position.Y + 1000)
                        bFocusPlayer = true;

                if (enemyLane[i].Count > 0)
                    if (enemyLane[i][0].Position.Y <= arrowContainer[i].Position.Y)
                        bFocusEnemy = true;
            }

            musicTime.Restart();
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            try
            {
                for (int i = 0; i < playerLane.Count; i++)
                {
                    List<Texture> activeArrow = new List<Texture>();

                    switch (i)
                    {
                        case 0:
                            activeArrow = arrowLeft;
                            break;
                        case 1:
                            activeArrow = arrowDown;
                            break;
                        case 2:
                            activeArrow = arrowUp;
                            break;
                        case 3:
                            activeArrow = arrowRight;
                            break;
                    }

                    if (Keyboard.IsKeyPressed(keys[i]))
                    {
                        arrowContainer[i].TextureRect = new IntRect(0, 0, (int)activeArrow[1].Size.X, (int)activeArrow[1].Size.Y);
                        arrowContainer[i].Texture = activeArrow[1];

                        foreach (var s in playerLane[i])
                            if (s.Position.Y <= arrowContainer[i].Position.Y + 100 * scrollSpeed && !bPressed[i])
                            {
                                bPressed[i] = true;

                                if (s.Position.Y <= arrowContainer[i].Position.Y + 25 * scrollSpeed)
                                    score += 100;
                                else if (s.Position.Y <= arrowContainer[i].Position.Y + 50 * scrollSpeed)
                                    score += 50;
                                else if (s.Position.Y <= arrowContainer[i].Position.Y + 100 * scrollSpeed)
                                    score += 25;

                                playerLane[i].Remove(s);

                                currPlayerPos = i;

                                scoreText.DisplayedString = "Score: " + score;
                                scoreText.Position = new Vector2f(1280 / 2 - scoreText.GetLocalBounds().Width, 0);

                                gameTick++;
                            }
                    }
                    else
                    {
                        bPressed[i] = false;
                        arrowContainer[i].TextureRect = new IntRect(0, 0, (int)activeArrow[0].Size.X, (int)activeArrow[0].Size.Y);
                        arrowContainer[i].Texture = activeArrow[0];
                    }
                }
            }

            catch { }

                if (instrumental.Status == 0 && bNext)
                    stack.ChangeStack(new PlayState(wnd, nWeek, nDay + 1, nDifficulty));

                if (instrumental.Status == 0 && bNext == false && nDay >= 1)
                    stack.ChangeStack(new SelectScreen(wnd));
            }
    }
}
