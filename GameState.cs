﻿using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using System.Text;

namespace FunnyFriday
{

    abstract class GameState : IDisposable
    {
        public abstract void Draw();

        public abstract void Update(ref Clock deltaTime);

        public abstract void InputHandling(StateMachine stack, ref Clock deltaTime);

        public void Dispose()
        {
            GC.Collect();
        }
    }

    class Intro : GameState
    {
        RenderWindow wnd;
        public Font regularFont;
        public Text regularText;
        public Text secondText;
        bool bFinished = false;


        public Intro(RenderWindow _wnd)
        {
            wnd = _wnd;

            regularFont = new Font("Assets/Fonts/komika-display.regular.ttf");
            regularText = new Text("", regularFont);
            regularText.LetterSpacing = 2.0f;
            regularText.CharacterSize = 40;
            regularText.FillColor = Color.White;
            regularText.Position = new Vector2f(wnd.Size.X / 2 - regularText.GetLocalBounds().Width / 2,
                wnd.Size.Y / 2 - regularText.GetLocalBounds().Height / 2 - 100);

            regularText.OutlineColor = new Color(20, 20, 20);
            regularText.OutlineThickness = 6.0f;

            secondText = new Text(regularText);
            secondText.DisplayedString = "";
            secondText.Position = new Vector2f(wnd.Size.X / 2 - secondText.GetLocalBounds().Width / 2,
                wnd.Size.Y / 2 - secondText.GetLocalBounds().Height / 2 - 50);
        }

        public override void Update(ref Clock deltaTime)
        {
            if (deltaTime.ElapsedTime.AsSeconds() >= 0.7f && deltaTime.ElapsedTime.AsSeconds() <= 2.0f)
            {
                regularText.DisplayedString = "FUNNY FRIDAY";
                regularText.Position = new Vector2f(wnd.Size.X / 2 - regularText.GetLocalBounds().Width / 2,
                    wnd.Size.Y / 2 - regularText.GetLocalBounds().Height / 2 - 100);
            }

            if (deltaTime.ElapsedTime.AsSeconds() >= 2.0f && deltaTime.ElapsedTime.AsSeconds() <= 3.0f)
            {
                secondText.DisplayedString = "LOL";
                secondText.Position = new Vector2f(wnd.Size.X / 2 - secondText.GetLocalBounds().Width / 2,
                    wnd.Size.Y / 2 - secondText.GetLocalBounds().Height / 2 - 50);
            }

            if (deltaTime.ElapsedTime.AsSeconds() >= 3.0f && deltaTime.ElapsedTime.AsSeconds() <= 4.1f)
            {
                regularText.DisplayedString = "";
                secondText.DisplayedString = "";
            }

            if (deltaTime.ElapsedTime.AsSeconds() >= 4.1f && deltaTime.ElapsedTime.AsSeconds() <= 5.4f)
            {
                regularText.DisplayedString = "FUCK YOU EGGER";
                regularText.Position = new Vector2f(wnd.Size.X / 2 - regularText.GetLocalBounds().Width / 2,
                    wnd.Size.Y / 2 - regularText.GetLocalBounds().Height / 2 - 100);
            }

            if(deltaTime.ElapsedTime.AsSeconds() >= 5.4f && deltaTime.ElapsedTime.AsSeconds() <= 6.5f)
            {
                secondText.DisplayedString = "GET IT IS TRASH";
                secondText.Position = new Vector2f(wnd.Size.X / 2 - secondText.GetLocalBounds().Width / 2,
                    wnd.Size.Y / 2 - secondText.GetLocalBounds().Height / 2 - 50);
            }

            if(deltaTime.ElapsedTime.AsSeconds() > 6.5)
            {
                bFinished = true;
            }
        }

        public override void Draw()
        {
            wnd.Draw(regularText);
            wnd.Draw(secondText);
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) || bFinished)
            {
                stack.ChangeStack(new TitleScreen(wnd));
            }
        }
    }

    class TitleScreen : GameState
    {
        private bool bTransition = false;
        private bool bFinished = false;
        private RenderWindow wnd;
        private Dictionary<string, Sprite> spriteContainer = new Dictionary<string, Sprite>();
        private List<Texture> logoTexture;
        private List<Texture> titleEnter;
        private List<Texture> titlePressed;
        RectangleShape transition;
        RectangleShape flash;
        int gameTick = 0;



        public TitleScreen(RenderWindow _wnd)
        {
            wnd = _wnd;

            TextureManager textureManager;

            textureManager = new TextureManager("Assets/XML/logoBumpin.xml");

            logoTexture = textureManager.GetTextures(4, 18);

            textureManager = new TextureManager("Assets/XML/titleEnter.xml");

            titleEnter = textureManager.GetTextures(13, 57);

            titlePressed = textureManager.GetTextures(4, 12);


            spriteContainer.Add("titleEnter", new Sprite(titleEnter[0]));
            spriteContainer["titleEnter"].Position = new Vector2f(150, 600.0f);

            spriteContainer.Add("logo", new Sprite(logoTexture[0]));
            spriteContainer["logo"].Position = new Vector2f(-60.0f, -100.0f);

            transition = new RectangleShape(new Vector2f(wnd.Size.X, wnd.Size.Y));
            transition.FillColor = new Color(0, 0, 0, 0);

            flash = new RectangleShape(new Vector2f(wnd.Size.X, wnd.Size.Y));
            flash.FillColor = new Color(255, 255, 255, 255);
        }

        override public void Draw()
        {
            wnd.Draw(spriteContainer["logo"]);
            wnd.Draw(spriteContainer["titleEnter"]);
            wnd.Draw(flash);
            wnd.Draw(transition);
        }

        public override void Update(ref Clock deltaTime)
        {
            if (flash.FillColor.A > 0)
                flash.FillColor = new Color(255, 255, 255, (byte)(flash.FillColor.A - 1));


            if (deltaTime.ElapsedTime.AsSeconds() >= 0.037f)
            {

                int index = gameTick % logoTexture.Count;
                spriteContainer["logo"].TextureRect = new IntRect(0, 0, (int)logoTexture[index].Size.X,
                    (int)logoTexture[index].Size.Y);
                spriteContainer["logo"].Texture = logoTexture[index];

                if(bTransition == false)
                {
                    int index1 = gameTick % titleEnter.Count;
                    spriteContainer["titleEnter"].TextureRect = new IntRect(0, 0, (int)titleEnter[index1].Size.X,
                        (int)titleEnter[index1].Size.Y);
                    spriteContainer["titleEnter"].Texture = titleEnter[index1];
                }
                

                gameTick++;
                deltaTime.Restart();
            }

            if (bTransition == true)
            {
                int index1 = gameTick % titlePressed.Count;
                spriteContainer["titleEnter"].TextureRect = new IntRect(0, 0, (int)titlePressed[index1].Size.X,
                    (int)titlePressed[index1].Size.Y);
                spriteContainer["titleEnter"].Texture = titlePressed[index1];

                if (transition.FillColor.A < 250)
                {
                    transition.FillColor = new Color(0, 0, 0, (byte)(transition.FillColor.A + 4));
                }   
                else
                {
                    bTransition = false;
                    bFinished = true;
                }

                gameTick++;
                deltaTime.Restart();
            }
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) && bTransition == false)
            {
                bTransition = true;
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/confirmMenu.ogg"));
                sound1.Play();
            }

            if (bFinished == true)
                stack.ChangeStack(new SelectScreen(wnd));
        }
    }

    class SelectScreen : GameState
    {
        RenderWindow wnd;
        public static List<Texture> storySelected;
        public static List<Texture> storyWhite;

        public static List<Texture> optionsSelected;
        public static List<Texture> optionsWhite;

        private Dictionary<string, Sprite> spriteContainer = new Dictionary<string, Sprite>();

        RectangleShape transition;
        int gameTick = 0;

        bool bTransition = false;
        bool bFinished = false;

        int selectedMenu = 0;

        public SelectScreen(RenderWindow _wnd)
        {
            wnd = _wnd;
            var textureManager = new TextureManager("Assets/XML/main_menu.xml");

            storySelected = textureManager.GetTextures(new string[] { "story mode basic0000", "story mode basic0001", "story mode basic0002",
            "story mode basic0003","story mode basic0004","story mode basic0005","story mode basic0006","story mode basic0007", "story mode basic0008"});

            storyWhite = textureManager.GetTextures(new string[] { "story mode white0000", "story mode white0001", "story mode white0002" });

            optionsSelected = textureManager.GetTextures(new string[] { "options basic0000", "options basic0001", "options basic0002",
            "options basic0003","options basic0004","options basic0005","options basic0006","options basic0007", "options basic0008"});

            optionsWhite = textureManager.GetTextures(new string[] { "options white0000", "options white0001", "options white0002" });

            transition = new RectangleShape(new Vector2f(wnd.Size.X, wnd.Size.Y));
            transition.FillColor = new Color(0, 0, 0, 255);

            spriteContainer.Add("menuBackground", new Sprite(new Texture("Assets/menuBGMagenta.png")));
            spriteContainer["menuBackground"].Position = new Vector2f(0, 0);

            spriteContainer.Add("story", new Sprite(storySelected[0]));
            spriteContainer["story"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["story"].GetLocalBounds().Width / 2, 200);

            spriteContainer.Add("options", new Sprite(optionsWhite[0]));
            spriteContainer["options"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["options"].GetLocalBounds().Width / 2, 400);
        }

        public override void Update(ref Clock deltaTime)
        {
            if (transition.FillColor.A >= 4 && bTransition == false)
                transition.FillColor = new Color(0, 0, 0, (byte)(transition.FillColor.A - 4));

            if (selectedMenu == 0 && deltaTime.ElapsedTime.AsSeconds() >= 0.04f)
            {
                int index = gameTick % storyWhite.Count;

                spriteContainer["story"].TextureRect = new IntRect(0, 0, (int)storyWhite[index].Size.X,
                    (int)storyWhite[index].Size.Y);
                spriteContainer["story"].Texture = storyWhite[index];
                spriteContainer["story"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["story"].GetLocalBounds().Width / 2, 200);


                int index1 = gameTick % optionsSelected.Count;

                spriteContainer["options"].TextureRect = new IntRect(0, 0, (int)optionsSelected[index1].Size.X,
                    (int)optionsSelected[index1].Size.Y);
                spriteContainer["options"].Texture = optionsSelected[index1];
                spriteContainer["options"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["options"].GetLocalBounds().Width / 2, 400);


                gameTick++;
                deltaTime.Restart();
            }

            if (selectedMenu == 1 && deltaTime.ElapsedTime.AsSeconds() >= 0.04f)
            {
                int index = gameTick % storySelected.Count;

                spriteContainer["story"].TextureRect = new IntRect(0, 0, (int)storySelected[index].Size.X,
                    (int)storySelected[index].Size.Y);
                spriteContainer["story"].Texture = storySelected[index];
                spriteContainer["story"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["story"].GetLocalBounds().Width / 2, 200);


                int index1 = gameTick % optionsWhite.Count;

                spriteContainer["options"].TextureRect = new IntRect(0, 0, (int)optionsWhite[index1].Size.X,
                    (int)optionsWhite[index1].Size.Y);
                spriteContainer["options"].Texture = optionsWhite[index1];
                spriteContainer["options"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["options"].GetLocalBounds().Width / 2, 400);


                gameTick++;
                deltaTime.Restart();
            }

            if (bTransition == true)
            {
                if (transition.FillColor.A < 250)
                {
                    transition.FillColor = new Color(0, 0, 0, (byte)(transition.FillColor.A + 4));
                }
                else
                {
                    bTransition = false;
                    bFinished = true;
                }

                gameTick++;
                deltaTime.Restart();
            }
        }

        public override void Draw()
        {
            wnd.Draw(spriteContainer["menuBackground"]);
            wnd.Draw(spriteContainer["story"]);
            wnd.Draw(spriteContainer["options"]);
            wnd.Draw(transition);
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && selectedMenu < 1)
            {
                spriteContainer["options"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["options"].GetLocalBounds().Width / 2, 400);
                selectedMenu = 1;
                spriteContainer["menuBackground"].Position = new Vector2f(0, -2);

                Sound scroll = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                scroll.Play();
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && selectedMenu > 0)
            {
                spriteContainer["story"].Position = new Vector2f(wnd.Size.X / 2 - spriteContainer["story"].GetLocalBounds().Width / 2, 200);
                selectedMenu = 0;
                spriteContainer["menuBackground"].Position = new Vector2f(0, 0);
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                sound1.Play();
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) && bTransition == false)
            {
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/confirmMenu.ogg"));
                sound1.Play();
                bTransition = true;
            }

            if(bFinished)
            {
                stack.ChangeStack(new WeekSelect(wnd));
            }
        }
    }

    class WeekSelect : GameState
    {
        RenderWindow wnd;
        private List<Sprite> weekContainer = new List<Sprite>();
        private Dictionary<string, Sprite> spriteContainer = new Dictionary<string, Sprite>();
        private List<Texture> difficultyTextures; 

        int difficultyChoice = 0;
        int weekChoice = 0;
        bool bLeft = false;
        bool bRight = false;

        RectangleShape leftSide;
        RectangleShape rightSide;
        RectangleShape top;

        string[] weekInfo;
        Font regularFont;
        Text regularText;
        List<Text> textContainer = new List<Text>();

        public WeekSelect(RenderWindow _wnd)
        {
            weekInfo = File.ReadAllLines("Assets/weekInfo.txt");
            regularFont = new Font("Assets/Fonts/vcr.ttf");

            wnd = _wnd;

            for (int i = 0; i < weekInfo.Length; i++)
            {
                weekContainer.Add(new Sprite(new Texture($"Assets/storymenu/week{i}.png")));
                weekContainer[i].Position = new Vector2f(wnd.Size.X / 2 - weekContainer[i].GetLocalBounds().Width / 2 + (weekContainer[i].GetLocalBounds().Width * i) + 100 * i, 400);
            }

            var textureManager = new TextureManager("Assets/XML/campainAssets.xml");

            difficultyTextures = textureManager.GetTextures(new string[] { "EASY0000", "NORMAL0000", "HARD0000", "arrow push right0000" });

            


            leftSide = new RectangleShape(new Vector2f(415, 300));
            leftSide.FillColor = new Color(0, 0, 0, 255);
            leftSide.Position = new Vector2f(0, 350);

            rightSide = new RectangleShape(new Vector2f(400, 300));
            rightSide.FillColor = new Color(0, 0, 0, 255);
            rightSide.Position = new Vector2f(885, 350);

            spriteContainer.Add("easy", new Sprite(difficultyTextures[0]));
            spriteContainer["easy"].Scale = new Vector2f(0.8f, 0.8f);
            spriteContainer["easy"].Position = new Vector2f(rightSide.Position.X + 100, 400);

            spriteContainer.Add("normal", new Sprite(difficultyTextures[1]));
            spriteContainer["normal"].Scale = new Vector2f(0.8f, 0.8f);
            spriteContainer["normal"].Position = new Vector2f(rightSide.Position.X + 100, 500);

            spriteContainer.Add("hard", new Sprite(difficultyTextures[2]));
            spriteContainer["hard"].Scale = new Vector2f(0.8f, 0.8f);
            spriteContainer["hard"].Position = new Vector2f(rightSide.Position.X + 100, 600);

            spriteContainer.Add("arrow", new Sprite(difficultyTextures[3]));
            spriteContainer["arrow"].Scale = new Vector2f(0.8f, 0.8f);
            spriteContainer["arrow"].Position = new Vector2f(rightSide.Position.X + 20, 400);

            top = new RectangleShape(new Vector2f(wnd.Size.X, 320));
            top.FillColor = new Color(255, 69, 0, 255);
            top.Position = new Vector2f(0, 50);

            textContainer.Add(new Text(weekInfo[0].Split(',')[0], regularFont));
            textContainer[0].Position = new Vector2f(wnd.Size.X - textContainer[0].GetLocalBounds().Width - 20, 0);

            textContainer.Add(new Text(weekInfo[0].Split(',')[1].Replace('-', '\n'), regularFont));
            textContainer[1].Position = new Vector2f(textContainer[0].GetLocalBounds().Width / 2, 450);

            textContainer.Add(new Text("Week Score: " + weekInfo[0].Split(',')[textContainer.Count], regularFont));
            textContainer[2].Position = new Vector2f(20, 0);
        }

        public override void Draw()
        {
            foreach (Sprite s in weekContainer)
                wnd.Draw(s);

            wnd.Draw(leftSide);
            wnd.Draw(rightSide);
            wnd.Draw(top);

            foreach (var s in spriteContainer)
                wnd.Draw(s.Value);

            foreach (var s in textContainer)
                wnd.Draw(s);
        }

        public override void Update(ref Clock deltaTime)
        {
            textContainer[0].DisplayedString = weekInfo[weekChoice].Split(',')[0];
            textContainer[0].Position = new Vector2f(wnd.Size.X - textContainer[0].GetLocalBounds().Width - 50, 0);

            textContainer[1].DisplayedString = weekInfo[weekChoice].Split(',')[1].Replace('-', '\n');
            textContainer[1].Position = new Vector2f(textContainer[0].GetLocalBounds().Width / 2, 450);


            textContainer[2].DisplayedString = "Week Score: " + weekInfo[weekChoice].Split(',')[textContainer.Count - 1];
            textContainer[2].Position = new Vector2f(20, 0);

            if (bLeft && weekContainer[weekChoice].Position.X <= (wnd.Size.X / 2 - weekContainer[weekChoice].GetLocalBounds().Width / 2))
            {
                foreach (Sprite week in weekContainer)
                    week.Position = new Vector2f(week.Position.X + 25, 400);
            }

            if (bRight && weekContainer[weekChoice].Position.X >= (wnd.Size.X / 2 - weekContainer[weekChoice].GetLocalBounds().Width / 2))
            {
                foreach (Sprite week in weekContainer)
                    week.Position = new Vector2f(week.Position.X - 25, 400);
            }

            switch(difficultyChoice)
            {
                case 0:
                    spriteContainer["arrow"].Position = new Vector2f(spriteContainer["arrow"].Position.X, spriteContainer["easy"].Position.Y);
                    break;

                case 1:
                    spriteContainer["arrow"].Position = new Vector2f(spriteContainer["arrow"].Position.X, spriteContainer["normal"].Position.Y);
                    break;
                case 2:
                    spriteContainer["arrow"].Position = new Vector2f(spriteContainer["arrow"].Position.X, spriteContainer["hard"].Position.Y);
                    break;
            }
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (deltaTime.ElapsedTime.AsSeconds() >= 0.2f && Keyboard.IsKeyPressed(Keyboard.Key.Right) && weekChoice < weekContainer.Count - 1)
            {
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                sound1.Play();
                weekChoice++;
                bRight = true; bLeft = false;
                deltaTime.Restart();
            }


            if (deltaTime.ElapsedTime.AsSeconds() >= 0.2f && Keyboard.IsKeyPressed(Keyboard.Key.Left) && weekChoice > 0)
            {
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                sound1.Play();
                weekChoice--;
                bRight = false; bLeft = true;
                deltaTime.Restart();
            }

            if (deltaTime.ElapsedTime.AsSeconds() >= 0.2f && Keyboard.IsKeyPressed(Keyboard.Key.Up) && difficultyChoice > 0)
            {
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                sound1.Play();
                difficultyChoice--;
                deltaTime.Restart();
            }

            if (deltaTime.ElapsedTime.AsSeconds() >= 0.2f && Keyboard.IsKeyPressed(Keyboard.Key.Down) && difficultyChoice < 2)
            {
                Sound sound1 = new Sound(new SoundBuffer("Assets/Sounds/scrollMenu.ogg"));
                sound1.Play();
                difficultyChoice++;
                deltaTime.Restart();
            }
        }
    }

    class Gameplay : GameState
    {
        #region Vars
        RenderWindow wnd;
        int gameTick = 0;
        int nWeek = 0;
        int nDifficulty = 0;
        float scrollSpeed = 1.5f;

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

        string[] weekFile;

        bool bLeft = false;
        bool bDown = false;
        bool bUp = false;
        bool bRight = false;

        Song currentSong;

        Sound instrumental;
        Sound voice;

        #endregion

        public Gameplay(RenderWindow _wnd, int week, int difficulty)
        {
            wnd = _wnd;
            nWeek = week;
            nDifficulty = difficulty;

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

            switch (nWeek)
            {
                case 0:
                    {
                        weekFile = File.ReadAllLines("Assets/Data/week1.txt");
                    }
                    break;
            }

            for (int i = 0; i < 4; i++)
            {
                enemyLane.Add(new List<Sprite>());
                playerLane.Add(new List<Sprite>());
            }

            var chart = new ChartParser("madness");
            currentSong = chart.GetSong();

            for (int i = 0; i < currentSong.song.notes.Length; i++)
                for (int j = 0; j < currentSong.song.notes[i].sectionNotes.Count; j++)
                    for (int k = 0; k < currentSong.song.notes[i].sectionNotes[j].Length; k++)
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

            instrumental = new Sound(new SoundBuffer("Assets/Music/Bopeebo/Inst.ogg"));
            voice = new Sound(new SoundBuffer("Assets/Music/Bopeebo/Voices.ogg"));

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

            
        }

        public override void Update(ref Clock deltaTime)
        {
            var delta = musicTime.ElapsedTime.AsSeconds() * 1000.0f * scrollSpeed;
            if (delta <= 16.0f  * scrollSpeed)
                delta = 16.0f * scrollSpeed;

            try
            {
                for (int i = 0; i < playerLane.Count; i++)
                    for (int j = 0; j < playerLane[i].Count; j++)
                    {
                        playerLane[i][j].Position = new Vector2f(playerLane[i][j].Position.X, playerLane[i][j].Position.Y - delta);

                        if(playerLane[i][j].Position.Y <= arrowContainer[i].Position.Y - 175.0f)
                            playerLane[i].Remove(playerLane[i][j]);
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

            if (animationTime.ElapsedTime.AsSeconds() >= 0.04f)
            {
                spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)playerIdle[gameTick % playerIdle.Count].Size.X, (int)playerIdle[gameTick % playerIdle.Count].Size.Y);
                spriteContainer["player"].Texture = playerIdle[gameTick % playerIdle.Count];
                gameTick++;
                animationTime.Restart();
            }
        }

        public override void InputHandling(StateMachine stack, ref Clock deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Y) && deltaTime.ElapsedTime.AsSeconds() >= 0.01f)
            {
                var activeArrow = arrowLeft[1];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;

                if (playerLane[0].Count > 0)
                    if (playerLane[0][0].Position.Y <= arrowContainer[0].Position.Y + 100 * scrollSpeed)
                    {
                        playerLane[0].RemoveAt(0);

                        var activeTexture = playerLeft;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        gameTick++;
                        deltaTime.Restart();
                    }
            }
            else
            {
                bLeft = false;
                var activeArrow = arrowLeft[0];
                arrowContainer[0].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[0].Texture = activeArrow;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.X) && deltaTime.ElapsedTime.AsSeconds() >= 0.01f)
            {
                var activeArrow = arrowDown[1];
                arrowContainer[1].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[1].Texture = activeArrow;

                if (playerLane[1].Count > 0)
                    if (playerLane[1][0].Position.Y <= arrowContainer[1].Position.Y + 100 * scrollSpeed)
                    {
                        playerLane[1].RemoveAt(0);

                        var activeTexture = playerDown;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        gameTick++;
                        deltaTime.Restart();
                    }
            }
            else
            {
                var activeArrow = arrowDown[0];
                arrowContainer[1].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[1].Texture = activeArrow;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Comma) && deltaTime.ElapsedTime.AsSeconds() >= 0.01f)
            {
                var activeArrow = arrowUp[1];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;

                if (playerLane[2].Count > 0)
                    if (playerLane[2][0].Position.Y  <= arrowContainer[2].Position.Y + 100 * scrollSpeed)
                    {
                        playerLane[2].RemoveAt(0);

                        var activeTexture = playerUp;
                        var index = gameTick % activeTexture.Count;

                        spriteContainer["player"].TextureRect = new IntRect(0, 0, (int)activeTexture[index].Size.X, (int)activeTexture[index].Size.Y);
                        spriteContainer["player"].Texture = activeTexture[index];

                        gameTick++;
                        deltaTime.Restart();
                    }
            }
            else
            {
                bUp = false;
                var activeArrow = arrowUp[0];
                arrowContainer[2].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[2].Texture = activeArrow;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.Period) && deltaTime.ElapsedTime.AsSeconds() >= 0.01f)
            {
                var activeArrow = arrowRight[1];
                arrowContainer[3].TextureRect = new IntRect(0, 0, (int)activeArrow.Size.X, (int)activeArrow.Size.Y);
                arrowContainer[3].Texture = activeArrow;

                if (playerLane[3].Count > 0)
                    if (playerLane[3][0].Position.Y <= arrowContainer[3].Position.Y + 100 * scrollSpeed)
                    {
                        playerLane[3].RemoveAt(0);

                        gameTick++;
                        deltaTime.Restart();
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
