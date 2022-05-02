using AmazingUILibrary;
using AmazingUILibrary.Backgrounds;
using AmazingUILibrary.Containers;
using AmazingUILibrary.Drawing;
using AmazingUILibrary.Elements;
using DisposeGame.Components;
using DisposeGame.Scripts;
using DisposeGame.Scripts.Bonuses;
using DisposeGame.Scripts.Character;
using DisposeGame.Scripts.Character.Player;
using DisposeGame.Scripts.Environment;
using GameEngine.Animation;
using GameEngine.Collisions;
using GameEngine.Game;
using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX;
using SharpDX.Mathematics.Interop;
using Sound;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DisposeGame.Scenes
{
    public class GameScene : Scene
    {
        private Camera _camera;

        private Game3DObject _player;
        private Game3DObject _rooms;

        private List<Game3DObject> _roads;
        private const float _roadOffset = 34.4f;
        private const float _roadDestroyBorder = -144.8f;

        private const float _leftRoadBorder = 90f;
        private const float _rightRoadBorder = 110f;

        private List<Game3DObject> _obstacles;
        private List<Game3DObject> _bonuses;
        private Stopwatch _obstacleTimer = new Stopwatch();
        private Stopwatch _bonusTimer = new Stopwatch();
        private const int _spawnObstacleTime = 2;
        private const int _spawnBonusTime = 4;

        private UIProgressBar _healthBar;
        private UIText _ammoCounter;

        private SharpAudioVoice _bonusPickedSound;
        private SharpAudioVoice _heroDeathSound;
        private SharpAudioVoice _heroHitedSound;
        private SharpAudioVoice _heroJump;
        private SharpAudioVoice _shotSound;
        private SharpAudioVoice _zombieDeathSound;
        private SharpAudioVoice _zombieHitedSound;

        private Loader _loader;
        private Random _random = new Random();

        public int TotalEnemies { get; set; }

        public override void Update(float delta)
        {
            base.Update(delta);
            CheckIIfPlayerOutsideOfRoad();
            SpawnObstacle(_obstacles, _obstacleTimer, _spawnObstacleTime);
            SpawnObstacle(_bonuses, _bonusTimer, _spawnBonusTime);
        }

        protected override void InitializeObjects(Loader loader, SharpAudioDevice audioDevice)
        {
            _loader = loader;
            _camera = new Camera(new Vector3(0, 22, -100), rotY: 0.20f, fovY: 0.1f);

            _bonusPickedSound = new SharpAudioVoice(audioDevice, @"Sounds\BonusPicked.wav");
            _heroDeathSound = new SharpAudioVoice(audioDevice, @"Sounds\HeroDeath.wav");
            _heroHitedSound = new SharpAudioVoice(audioDevice, @"Sounds\HeroHited.wav");
            _heroJump = new SharpAudioVoice(audioDevice, @"Sounds\HeroJump.wav");
            _shotSound = new SharpAudioVoice(audioDevice, @"Sounds\Shot.wav");
            _zombieDeathSound = new SharpAudioVoice(audioDevice, @"Sounds\ZombieDeath.wav");
            _zombieHitedSound = new SharpAudioVoice(audioDevice, @"Sounds\ZombieHited.wav");

            _rooms = CreateLevel(loader);

            _player = CreatePlayer(loader);
            _player.MoveTo(new Vector3(100, 1, -100));

            AddGameObject(_player);

            //AddGameObject(CreateZombie(loader, new Vector3(70, 0, 90)));

            //AddGameObject(CreateZombie(loader, new Vector3(165, 0, 165)));
            //AddGameObject(CreateZombie(loader, new Vector3(16.660, 0, 90)));
            //AddGameObject(CreateZombie(loader, new Vector3(230, 0, 130)));

            //AddGameObject(CreateZombie(loader, new Vector3(165, 0, 0)));
            //AddGameObject(CreateZombie(loader, new Vector3(200, 0, 0)));
            //AddGameObject(CreateZombie(loader, new Vector3(240, 0, 0)));
            //AddGameObject(CreateZombie(loader, new Vector3(205, 0, -30)));
            //AddGameObject(CreateZombie(loader, new Vector3(16.660, 0, -30)));

            //AddGameObject(CreateZombie(loader, new Vector3(70, 0, 190)));
            //AddGameObject(CreateZombie(loader, new Vector3(90, 0, 240)));

            //AddGameObject(CreateZombie(loader, new Vector3(16.660, 0, 295)));
            //AddGameObject(CreateZombie(loader, new Vector3(230, 0, 275)));

            //AddGameObject(CreateZombie(loader, new Vector3(300, 0, 150)));
            //AddGameObject(CreateZombie(loader, new Vector3(300, 0, 35)));

            //AddGameObject(CreateAmmoBonus(loader, new Vector3(15, 0, 105)));
            //AddGameObject(CreateInvisibilityBonus(loader, new Vector3(120, 0, 80)));

            //AddGameObject(CreateAmmoBonus(loader, new Vector3(200, 0, 40)));
            //AddGameObject(CreateAmmoBonus(loader, new Vector3(220, 0, 40)));
            //AddGameObject(CreateHealthBonus(loader, new Vector3(16.660, 0, 40)));

            _roads = new List<Game3DObject>();
            var roadOne = CreateRoad(loader, new Vector3(100, 0, -110));
            var grassOneLeft = CreateGrass(loader, new Vector3(roadOne.Position.X - 16.66f, roadOne.Position.Y, roadOne.Position.Z));
            var grassOneRight = CreateGrass(loader, new Vector3(roadOne.Position.X + 16.66f, roadOne.Position.Y, roadOne.Position.Z));
            _roads.Add(roadOne);
            var roadTwo = CreateRoad(loader, new Vector3(100, 0, -75.2f));
            var grassTwoLeft = CreateGrass(loader, new Vector3(roadTwo.Position.X - 16.66f, roadTwo.Position.Y, roadTwo.Position.Z));
            var grassTwoRight = CreateGrass(loader, new Vector3(roadTwo.Position.X + 16.66f, roadTwo.Position.Y, roadTwo.Position.Z));
            _roads.Add(roadTwo);
            var roadThree = CreateRoad(loader, new Vector3(100, 0, -40.4f));
            var grassThreeLeft = CreateGrass(loader, new Vector3(roadThree.Position.X - 16.66f, roadThree.Position.Y, roadThree.Position.Z));
            var grassThreeRight = CreateGrass(loader, new Vector3(roadThree.Position.X + 16.66f, roadThree.Position.Y, roadThree.Position.Z));
            _roads.Add(roadThree);
            var roadFour = CreateRoad(loader, new Vector3(100, 0, -5.6f));
            var grassFourLeft = CreateGrass(loader, new Vector3(roadFour.Position.X - 16.66f, roadFour.Position.Y, roadFour.Position.Z));
            var grassFourRight = CreateGrass(loader, new Vector3(roadFour.Position.X + 16.66f, roadFour.Position.Y, roadFour.Position.Z));
            _roads.Add(roadFour);

            foreach (var road in _roads)
            {
                AddGameObject(road);
            }

            AddGameObject(grassOneLeft);
            AddGameObject(grassOneRight);
            AddGameObject(grassTwoLeft);
            AddGameObject(grassTwoRight);
            AddGameObject(grassThreeLeft);
            AddGameObject(grassThreeRight);
            AddGameObject(grassFourLeft);
            AddGameObject(grassFourRight);

            _obstacles = new List<Game3DObject>();
            var car1 = CreateOncomingCar(loader, @"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\policecar2.fbx");
            var car2 = CreateFollowingCar(loader, @"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\policecar2.fbx");

            _obstacles.Add(car1);
            _obstacles.Add(car2);

            AddGameObject(car1);
            AddGameObject(car2);

            _bonuses = new List<Game3DObject>();
            var bonus1 = CreateHealthBonus(loader, new Vector3(100, 1, -80));
            var bonus2 = CreateHealthBonus(loader, new Vector3(90, 1, -80));
            _bonuses.Add(bonus1);
            _bonuses.Add(bonus2);
            AddGameObject(bonus1);
            AddGameObject(bonus2);


            _obstacleTimer.Start();
            _bonusTimer.Start();
        }

        private Game3DObject CreateHealthBonus(Loader loader, Vector3 position)
        {
            var health = CreateBonus(loader, @"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\newheart9.fbx", new HealthBonusScript(_player));
            health.MoveTo(position);
            return health;
        }

        private Game3DObject CreateOncomingCar(Loader loader, string modelPath)
        {
            var environmentScript = new EnvironmentScript(Vector3.UnitZ, -1f, _roadDestroyBorder);
            environmentScript.Destroy += FreeOncomingCar;
            var car = CreateEnvironment(loader, modelPath, environmentScript);
            var obstacleScript = new ObstacleScript(_player, 50);
            obstacleScript.OnPicked += FreeOncomingCar;
            car.AddScript(obstacleScript);
            var position = new Vector3(96, 1, -90);
            car.MoveTo(position);
            car.RotateZ(MathUtil.DegreesToRadians(180));
            car.Speed = -1f;
            car.Collision = new BoxCollision(1.7f, 1);
            car.AddComponent(new ObstacleInfoComponent { IsFree = true, OriginSpeed = -1f });
            return car;
        }

        private Game3DObject CreateFollowingCar(Loader loader, string modelPath)
        {
            var environmentScript = new EnvironmentScript(Vector3.UnitZ, -0.4f, _roadDestroyBorder);
            environmentScript.Destroy += FreeFollowingCar;
            var car = CreateEnvironment(loader, modelPath, environmentScript);
            var obstacleScript = new ObstacleScript(_player, 30);
            obstacleScript.OnPicked += FreeFollowingCar;
            car.AddScript(obstacleScript);
            var position = new Vector3(106, 1, -90);
            car.MoveTo(position);
            car.Speed = -0.4f;
            car.Collision = new BoxCollision(1.7f, 1);
            car.AddComponent(new ObstacleInfoComponent { IsFree = true, OriginSpeed = -0.4f });
            return car;
        }

        private Game3DObject CreateRoad(Loader loader, Vector3 position)
        {
            var environmentScript = new EnvironmentScript(Vector3.UnitZ, -0.5f, _roadDestroyBorder);
            environmentScript.Destroy += RemoveRoad;
            var road = CreateEnvironment(loader, @"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\road9.fbx", environmentScript);
            road.MoveTo(position);
            road.Speed = -0.5f;
            return road;
        }

        private Game3DObject CreateGrass(Loader loader, Vector3 position)
        {
            var environmentScript = new EnvironmentScript(Vector3.UnitZ, -0.5f, _roadDestroyBorder);
            environmentScript.Destroy += RemoveGrass;
            var road = CreateEnvironment(loader, @"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\grass4.fbx", environmentScript);
            road.MoveTo(position);
            road.Speed = -0.5f;
            return road;
        }

        private void RemoveRoad(Script sender, Game3DObject gameObject)
        {
            Game3DObject groundNearestToSpawnPoint = null;
            var maxValue = float.MinValue;
            foreach (var ground in _roads)
            {
                if (ground.Position.Z > maxValue)
                {
                    maxValue = ground.Position.Z;
                    groundNearestToSpawnPoint = ground;
                }
            }
            var newCoordZ = groundNearestToSpawnPoint.Position.Z + _roadOffset;
            gameObject.MoveTo(new Vector3(gameObject.Position.X, gameObject.Position.Y, newCoordZ));
        }

        private void FreeOncomingCar(Script sender, Game3DObject gameObject)
        {
            var newXPosition = _random.Next(94, 98);
            gameObject.MoveTo(new Vector3(newXPosition, gameObject.Position.Y, -4));
            gameObject.Speed = 0;
            gameObject.GetComponent<ObstacleInfoComponent>().IsFree = true;
        }

        private void FreeFollowingCar(Script sender, Game3DObject gameObject)
        {
            var newXPosition = _random.Next(104, 108);
            gameObject.MoveTo(new Vector3(newXPosition, gameObject.Position.Y, -4));
            gameObject.Speed = 0;
            gameObject.GetComponent<ObstacleInfoComponent>().IsFree = true;
        }
        private void FreeBonus(Script sender, Game3DObject gameObject)
        {
            var newXPosition = _random.Next(94, 108);
            gameObject.MoveTo(new Vector3(newXPosition, gameObject.Position.Y, -4));
            gameObject.Speed = 0;
            gameObject.GetComponent<ObstacleInfoComponent>().IsFree = true;
        }

        private void RemoveGrass(Script sender, Game3DObject gameObject)
        {
            Game3DObject groundNearestToSpawnPoint = null;
            var maxValue = float.MinValue;
            foreach (var ground in _roads)
            {
                if (ground.Position.Z > maxValue)
                {
                    maxValue = ground.Position.Z;
                    groundNearestToSpawnPoint = ground;
                }
            }
            var newCoordZ = groundNearestToSpawnPoint.Position.Z;
            gameObject.MoveTo(new Vector3(gameObject.Position.X, gameObject.Position.Y, newCoordZ));
        }

        private Game3DObject CreateAmmoBonus(Loader loader, Vector3 position)
        {
            var ammo = CreateBonus(loader, @"Models\BonusAmmo.fbx", new AmmoBonusScript(_player));
            ammo.MoveTo(position);
            return ammo;
        }

        private Game3DObject CreateInvisibilityBonus(Loader loader, Vector3 position)
        {
            var invisibility = CreateBonus(loader, @"Models\BonusStels.fbx", new InvisibilityBonusScript(_player));
            invisibility.MoveTo(position);
            return invisibility;
        }

        private Game3DObject CreateBonus(Loader loader, string path, PickableBonusScript script)
        {
            var bonus = loader.LoadGameObjectFromFile(path, Vector3.Zero, Vector3.Zero);
            bonus.Collision = new SphereCollision(2);
            bonus.Children[0].SetRotationZ(-MathUtil.PiOverTwo);
            var animation = new SmoothAnimation(new float[] { 0, MathUtil.TwoPi, 0 }, 1, int.MaxValue);
            animation.AddProcess(value => 
            {
                bonus.SetRotationZ(value);
                //bonus.SetPositionY(value - MathUtil.Pi);
            });
            script.OnPicked += (sender, gameObject) =>
            {
                _bonusPickedSound.Play();
            };
            script.OnPicked += FreeBonus;
            bonus.AddScript(script);
            var environmentScript = new EnvironmentScript(Vector3.UnitZ, -0.4f, _roadDestroyBorder);
            environmentScript.Destroy += FreeBonus;
            bonus.AddScript(environmentScript);
            bonus.Speed = -0.4f;
            bonus.AddComponent(new ObstacleInfoComponent { IsFree = true, OriginSpeed = -0.4f });
            return bonus;
        }

        private Game3DObject CreateEnvironment(Loader loader, string path, EnvironmentScript script)
        {
            var environment = loader.LoadGameObjectFromFile(path, Vector3.Zero, Vector3.Zero);
            environment.AddScript(script);
            return environment;
        }

        private Game3DObject CreateLevel(Loader loader)
        {
            var level = loader.LoadGameObjectFromFile(@"Models\wall.fbx", Vector3.UnitY * -14, Vector3.Zero);

            foreach (var child in level.Children)
            {
                child.SetRotationX(0);
                child.SetRotationY(0);
                child.SetRotationZ(0);

                var vertices = child.Mesh.Vertices.Select(_ => _.position);
                var minX = vertices.Select(_ => _.X).Min();
                var minY = vertices.Select(_ => _.Y).Min();
                var minZ = vertices.Select(_ => _.Z).Min();
                var maxX = vertices.Select(_ => _.X).Max();
                var maxY = vertices.Select(_ => _.Y).Max();
                var maxZ = vertices.Select(_ => _.Z).Max();

                child.Collision = new StaticBoxCollision(new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ)), vertices.ToList().Count);
            }

            return level;
        }

        private Game3DObject LoadPersonWithTexture(
            Loader loader, 
            string texture)
        {
            var body = loader.LoadGameObjectFromFile(@"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\mainCar61.fbx", Vector3.Zero, new Vector3(0, 0, 0));
            /*body.Children[0].SetRotationZ(-MathUtil.PiOverTwo);

            body.Children[0].Children[0].IsHidden = true;
            body.Children[0].Children[1].IsHidden = true;
            body.Children[0].Children[2].IsHidden = true;
            body.Children[0].Children[3].IsHidden = true;
            body.Children[0].Children[4].IsHidden = true;

            leftArm = body.Children[0].Children[0].Children[0];
            rightArm = body.Children[0].Children[1].Children[0];
            leftLeg = body.Children[0].Children[2].Children[0];
            rightLeg = body.Children[0].Children[3].Children[0];
            head = body.Children[0].Children[4].Children[0];*/

            return body;
        }

        private Game3DObject CreatePlayer(Loader loader)
        {
            var body = LoadPersonWithTexture(loader, @"Textures\gachi.png");

            var helpCube = loader.LoadGameObjectFromFile(@"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Models\helpcube2.fbx", Vector3.Zero, Vector3.Zero);
            //helpCube.MoveTo(new Vector3(100, 1, -100));
            helpCube.AddScript(new HelpCubeMovementScript());
            helpCube.AddChild(_camera);
            body.AddChild(helpCube);
            helpCube.MoveTo(new Vector3(0, 0.5f, 0));
            AddGameObject(helpCube);

            var gun = loader.MakeRectangle(Vector3.UnitY * -6, Vector3.Zero, Vector3.One * 1.2f);
            var bullet = loader.MakeRectangle(Vector3.Zero, Vector3.Zero, Vector3.One * 0.2f);
            bullet.Collision = new SphereCollision(1);

            //rightArm.AddChild(gun);

            //rightArm.SetRotationX(MathUtil.PiOverTwo);

            var characterMovementAnimation = new Animation(new float[] { 0, MathUtil.PiOverFour, 0, -MathUtil.PiOverFour, 0 }, 1, int.MaxValue);
            /*characterMovementAnimation.AddProcess(value =>
            {
                leftLeg.SetRotationX(value);
                rightLeg.SetRotationX(-value);
                leftArm.SetRotationX(-value);
                head.SetRotationZ(value);
            });
            characterMovementAnimation.AddTransitionPaused(() =>
            {
                leftLeg.SetRotationX(0);
                rightLeg.SetRotationX(0);
                leftArm.SetRotationX(0);
                head.SetRotationZ(0);
            });*/

            //var physics = new PhysicsComponent(_rooms.Children);
            //body.AddComponent(physics);
            //body.AddScript(new PhysicsScript(physics));

            var movementScript = new PlayerMovementScript(characterMovementAnimation, null, _rooms.Children);
            movementScript.OnJump += () => _heroJump.Play();
            body.AddScript(movementScript);

            var visibility = new VisibilityComponent();
            body.AddComponent(visibility);
            body.AddScript(new VisibilityScript(visibility));

            var health = new HealthComponent(100);
            _healthBar.MaxValue = 100;
            _healthBar.Value = 100;
            health.OnChanged += value => _healthBar.Value = value;
            health.OnDeath += () =>
            {
                _heroDeathSound.Play();
                this.RemoveGameObject(helpCube);
                Game.ChangeScene(new DeathMenuScene());
            };
            health.OnDamaged += (current, damage) =>
            {
                _heroHitedSound.Stop();
                _heroHitedSound.Play();
            };
            body.AddComponent(health);

            var ammo = new AmmoComponent();
            _ammoCounter.Text = ammo.Ammo.ToString();
            ammo.OnChanged += value => _ammoCounter.Text = value.ToString();
            ammo.OnSpended += value =>
            {
                _shotSound.Stop();
                _shotSound.Play();
            };
            body.AddComponent(ammo);

            body.AddScript(new PlayerUnbreakableScript(health));

            gun.AddScript(new PlayerGunScript(bullet, ammo, _rooms.Children));

            body.Collision = new BoxCollision(1.7f, 20);

            return body;
        }

        private Game3DObject CreateZombie(Loader loader, Vector3 position)
        {
            var body = LoadPersonWithTexture(loader, @"Textures\zombienew.png");


            Animation zombieIdleAnimation = new Animation(new float[] { 0, MathUtil.Pi / 16f, 0, -MathUtil.Pi / 16f, 0 }, 1, int.MaxValue);
            /*zombieIdleAnimation.AddProcess(value =>
            {
                head.SetRotationZ(value);
                head.SetRotationX(value);
                leftArm.SetRotationX(value + MathUtil.PiOverTwo);
                rightArm.SetRotationX(-value + MathUtil.PiOverTwo);
            });*/
            Animation movementAnimation = new Animation(new float[] { 0, MathUtil.PiOverFour, 0, -MathUtil.PiOverFour, 0 }, 1, int.MaxValue);
            /*movementAnimation.AddProcess(value =>
            {
                leftLeg.SetRotationX(value);
                rightLeg.SetRotationX(-value);
            });
            movementAnimation.AddTransitionPaused(() =>
            {
                leftLeg.SetRotationX(0);
                rightLeg.SetRotationX(0);
            });*/

            body.Collision = new BoxCollision(5, 20);
            body.AddScript(new ZombieMovementScript(_player, movementAnimation, _rooms.Children));
            var health = new HealthComponent(30);
            health.OnDeath += () => 
            {
                _zombieDeathSound.Play();
                TotalEnemies--;
                if (TotalEnemies <= 0)
                {
                    Game.ChangeScene(new WinMenuScene());
                }
            };
            health.OnDamaged += (current, damage) =>
            {
                _zombieHitedSound.Stop();
                _zombieHitedSound.Play();
            };
            body.AddComponent(health);

            body.MoveTo(position);

            TotalEnemies++;

            return body;
        }

        private void CheckIIfPlayerOutsideOfRoad()
        {
            if (_player.Position.X <= _leftRoadBorder || _player.Position.X >= _rightRoadBorder)
            {
                _player.GetComponent<HealthComponent>().DealDamage(10);
            }
        }

        private void SpawnObstacle(List<Game3DObject> obstacles, Stopwatch timer, float spawnObstacleTime)
        {
            if (timer.Elapsed.TotalSeconds >= spawnObstacleTime && obstacles.Any(obstacle => obstacle.GetComponent<ObstacleInfoComponent>().IsFree))
            {
                timer.Restart();
                var length = obstacles.Count;
                while (true)
                {
                    var index = _random.Next(0, length);
                    var obsatcleInfo = obstacles[index].GetComponent<ObstacleInfoComponent>();
                    if (obsatcleInfo.IsFree)
                    {
                        obstacles[index].Speed = obsatcleInfo.OriginSpeed;
                        obsatcleInfo.IsFree = false;
                        break;
                    }
                }
            }
        }

        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            context.NewNinePartsBitmap("glowingBorder", loader.LoadBitmapFromFile(@"Textures\GlowingBorder.png"), 21, 29, 21, 29);
            context.NewBitmap("bulletsTexture", loader.LoadBitmapFromFile(@"Textures\Bullets.png"));
            context.NewSolidBrush("neonBrush", new RawColor4(144f / 255f, 238f / 255f, 233f / 255f, 1f));
            context.NewTextFormat("ammoFormat", 
                fontWeight: SharpDX.DirectWrite.FontWeight.Black,
                textAlignment: SharpDX.DirectWrite.TextAlignment.Center,
                paragraphAlignment: SharpDX.DirectWrite.ParagraphAlignment.Center);

            var ui = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.End,
                CrossAxis = UISequentialContainer.Alignment.Start
            };
            
            _healthBar = new UIProgressBar(Vector2.Zero, new Vector2(100, 20), "neonBrush");
            _ammoCounter = new UIText("0", new Vector2(48, 10), "ammoFormat", "neonBrush");
            var ammoImage = new UIPanel(Vector2.Zero, new Vector2(40, 36))
            {
                Background = new TextureBackground("bulletsTexture")
            };

            var ammoContainer = new UISequentialContainer(Vector2.Zero, new Vector2(100, 36), false)
            {
                MainAxis = UISequentialContainer.Alignment.Start,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            var healthContainer = new UIMarginContainer(_healthBar, 15)
            {
                Background = new NinePartsTextureBackground("glowingBorder")
            };

            ammoContainer.Add(new UIMarginContainer(ammoImage, 6, 0));
            ammoContainer.Add(_ammoCounter);

            ui.Add(ammoContainer);
            ui.Add(healthContainer);

            return ui;
        }

        protected override Renderer.IlluminationProperties CreateIllumination()
        {
            Renderer.IlluminationProperties illumination = base.CreateIllumination();
            Renderer.LightSource lightSource = new Renderer.LightSource();
            lightSource.lightSourceType = Renderer.LightSourceType.Directional;
            lightSource.color = Vector3.One;
            lightSource.direction = Vector3.Normalize(new Vector3(0.5f, -2.0f, 1.0f));
            illumination[0] = lightSource;
            return illumination;
        }

        protected override Camera CreateCamera()
        {
            return _camera;
        }

        public override void Dispose()
        {
            _bonusPickedSound.Dispose();
            _heroDeathSound.Dispose();
            _heroHitedSound.Dispose();
            _heroJump.Dispose();
            _shotSound.Dispose();
            _zombieDeathSound.Dispose();
            _zombieHitedSound.Dispose();

            base.Dispose();

        }
    }
}
