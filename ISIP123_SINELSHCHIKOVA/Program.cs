using System;
using System.Collections.Generic;

namespace JustaGame
{
    class Program
    {
        static Random rng = new Random();

        static void Main(string[] args)
        {
            Player player = new Player("Герой", 100);

            int turn = 1;
            bool gameOver = false;

            Console.WriteLine("START");
            Console.WriteLine("Управление: Атака - 1, Защита - 2");
            Console.WriteLine();

            while (!gameOver)
            {
                Console.WriteLine($"--- Ход {turn} ---");

                // Каждые 10 ходов — босс
                if (turn % 10 == 0)
                {
                    Enemy boss = EnemyFactory.CreateRandomBoss();
                    Console.WriteLine($"Вы встретили БОССА: {boss.Name}!");
                    gameOver = !Battle(player, boss);
                }
                else
                {
                    if (rng.NextDouble() < 0.5)
                    {
                        // Враг
                        Enemy enemy = EnemyFactory.CreateRandomEnemy();
                        Console.WriteLine($"Вы встретили врага: {enemy.Name}");
                        gameOver = !Battle(player, enemy);
                    }
                    else
                    {
                        // Сундук
                        Console.WriteLine("Вы нашли сундук!");
                        Chest.Open(player);
                    }
                }

                if (player.HP <= 0)
                {
                    gameOver = true;
                    Console.WriteLine("Вы погибли! Игра окончена.");
                }

                turn++;
                Console.WriteLine();
            }
        }

        // Бой с врагом
        static bool Battle(Player player, Enemy enemy)
        {
            bool playerFrozen = false;

            while (player.HP > 0 && enemy.HP > 0)
            {
                Console.WriteLine($"\nВаш HP: {player.HP} | HP врага: {enemy.HP}");

                if (!playerFrozen)
                {
                    Console.WriteLine("Выберите действие: 1-Атака, 2-Защита");
                    string input = Console.ReadLine();
                    if (input == "1")
                    {
                        int damage = player.Attack() - enemy.Defense;
                        if (damage < 0) damage = 0;
                        enemy.HP -= damage;
                        Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}");
                    }
                    else
                    {
                        player.IsDefending = true;
                        Console.WriteLine("Готовьтесь к защите!");
                    }
                }
                else
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход");
                    playerFrozen = false;
                }

                // Проверка смерти врага
                if (enemy.HP <= 0)
                {
                    Console.WriteLine($"{enemy.Name} повержен!");
                    player.IsDefending = false;
                    return true;
                }

                // Ход врага
                int enemyDamage = enemy.Attack();
                bool specialEffect = false;

                // Особенности врага
                if (enemy is Goblin g && rng.NextDouble() < g.CritChance)
                {
                    enemyDamage *= 2;
                    Console.WriteLine("Критический удар");
                }
                else if (enemy is Mage m && rng.NextDouble() < m.FreezeChance)
                {
                    Console.WriteLine("Маг наложил заморозку!");
                    playerFrozen = true;
                    specialEffect = true;
                }
                else if (enemy is Skeleton s && s.IgnoreDefense)
                {
                    Console.WriteLine("Скелет игнорирует вашу броню");
                    playerDamage(player, enemyDamage, ignoreDefense: true);
                    continue;
                }

                if (!specialEffect)
                    playerDamage(player, enemyDamage);

                // Сброс защиты игрока после хода врага
                player.IsDefending = false;
            }

            return player.HP > 0;
        }

        static void playerDamage(Player player, int damage, bool ignoreDefense = false)
        {
            if (player.IsDefending)
            {
                if (rng.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы увернулись от атаки");
                    damage = 0;
                }
                else
                {
                    int block = rng.Next((int)(player.Defense * 0.7), player.Defense + 1);
                    damage -= ignoreDefense ? 0 : block;
                    if (damage < 0) damage = 0;
                    Console.WriteLine($"Вы блокировали {block} урона");
                }
            }

            player.HP -= damage;
            Console.WriteLine($"Враг наносит вам {damage} урона");
        }
    }

    class Player
    {
        public string Name;
        public int HP;
        public int Defense;
        public bool IsDefending = false;

        public Player(string name, int hp)
        {
            Name = name;
            HP = hp;
            Defense = 10;
        }

        public int Attack()
        {
            return 15; // Базовая атака
        }
    }

    abstract class Enemy
    {
        public string Name;
        public int HP;
        public int AttackValue;
        public int Defense;

        public virtual int Attack()
        {
            return AttackValue;
        }
    }

    class Goblin : Enemy
    {
        public double CritChance = 0.2;
    }

    class Skeleton : Enemy
    {
        public bool IgnoreDefense = true;
    }

    class Mage : Enemy
    {
        public double FreezeChance = 0.2;
    }

    class EnemyFactory
    {
        static Random rng = new Random();

        public static Enemy CreateRandomEnemy()
        {
            int r = rng.Next(3);
            switch (r)
            {
                case 0:
                    return new Goblin { Name = "Гоблин", HP = 30, AttackValue = 10, Defense = 5 };
                case 1:
                    return new Skeleton { Name = "Скелет", HP = 35, AttackValue = 12, Defense = 3 };
                default:
                    return new Mage { Name = "Маг", HP = 25, AttackValue = 8, Defense = 4 };
            }
        }

        public static Enemy CreateRandomBoss()
        {
            int r = rng.Next(3);
            switch (r)
            {
                case 0:
                    return new Goblin { Name = "БоссБосс", HP = 60, AttackValue = 15, Defense = 6, CritChance = 0.3 };
                case 1:
                    return new Skeleton { Name = "БоссБоссБосс", HP = 88, AttackValue = 16, Defense = 5, IgnoreDefense = true };
                default:
                    return new Mage { Name = "Босс", HP = 45, AttackValue = 13, Defense = 4, FreezeChance = 0.3 };
            }
        }
    }

    static class Chest
    {
        static Random rng = new Random();

        public static void Open(Player player)
        {
            int r = rng.Next(3);
            switch (r)
            {
                case 0:
                    Console.WriteLine("Вы нашли лечебное зелье! Полное восстановление");
                    player.HP = 100;
                    break;
                case 1:
                    Console.WriteLine("Вы нашли новое оружие!");
                    break;
                case 2:
                    Console.WriteLine("Вы нашли новые доспехи!");
                    break;
            }
        }
    }
}
