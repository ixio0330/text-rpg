bool IsGameRunning = true;
Console.WriteLine("캐릭터 이름을 입력해주세요.");
Console.WriteLine("※ 이름은 한번 설정하면 변경할 수 없으니 신중하게 선택해주세요.");
Player player = new Player(Console.ReadLine());

bool IsPlayerDead = false;
while (IsGameRunning)
{
    MAP map = IsPlayerDead ? MAP.VILLAGE : GameStart();

    switch (map)
    {
        case MAP.VILLAGE:
            EnterVillage(player);
            IsPlayerDead = false;
            break;
        case MAP.BATTLE:
            IsPlayerDead = EnterBattle(player) == BATTLE_RESULT.LOOSE;
            break;
        case MAP.END:
            IsGameRunning = false;
            break;
    }
}

static BATTLE_RESULT EnterBattle(Player player)
{
    bool IsEnterBattle = true;
    BATTLE_RESULT result = BATTLE_RESULT.NONE;
    while (IsEnterBattle)
    {
        Console.Clear();
        Monster monster = new Monster();
        Console.WriteLine("1. 공격하기");
        Console.WriteLine("2. 도망치기");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                Console.Clear();
                result = Fight(player, monster);
                if (result == BATTLE_RESULT.VICTORY)
                {
                    Console.WriteLine("몬스터를 처치했습니다!");
                    Console.WriteLine("사냥을 계속 하시겠습니까?");
                    Console.WriteLine("1. 계속하기");
                    Console.WriteLine("2. 돌아가기");
                    IsEnterBattle = Console.ReadKey().Key == ConsoleKey.D1;
                }
                else
                {
                    Console.WriteLine("죽어서 마을로 이동합니다.");
                    Console.ReadLine();
                    IsEnterBattle = false;
                }
                break;
            case ConsoleKey.D2:
                IsEnterBattle = false;
                break;
            default:
                IsEnterBattle = true;
                break;
        }
    }
    return result;
}

static BATTLE_RESULT Fight(Player player, Monster monster)
{
    while (player.IsAlive() & monster.IsAlive())
    {
        Charater.Attact(player, monster);
        Charater.Attact(monster, player);
    }
    return player.IsAlive() ? BATTLE_RESULT.VICTORY : BATTLE_RESULT.LOOSE;
}

static void EnterVillage(Player player)
{
    bool IsEnterVillage = true;
    while (IsEnterVillage)
    {
        Console.Clear();
        player.PrintInfo();
        Console.WriteLine("마을에서 무엇을 하시겠습니까?");
        Console.WriteLine("1. 체력 회복");
        Console.WriteLine("2. 공격력 강화");
        Console.WriteLine("3. 마을을 나간다.");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                player.MaxHealHP();
                break;
            case ConsoleKey.D2:
                player.UpAT();
                Console.WriteLine("공격력을 강화했습니다.");
                break;
            case ConsoleKey.D3:
                Console.WriteLine("마을을 빠져나갑니다.");
                IsEnterVillage = false;
                break;
            default:
                break;
        }
    }
}

static MAP GameStart()
{
    Console.Clear();
    Console.WriteLine("게임을 시작했습니다. 이동할 곳을 선택해주세요.");
    Console.WriteLine("1. 마을");
    Console.WriteLine("2. 사냥터");
    Console.WriteLine("3. 게임 종료");
    MAP map;
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.D1:
            map = MAP.VILLAGE;
            break;
        case ConsoleKey.D2:
            map = MAP.BATTLE;
            break;
        case ConsoleKey.D3:
            Console.WriteLine("아무 키나 누르면 게임을 종료됩니다.");
            Console.ReadLine();
            map = MAP.END;
            break;
        default:
            map = MAP.NONE;
            break;
    }
    return map;
}

enum MAP
{
    VILLAGE, // 마을
    BATTLE, // 사냥터
    NONE, // 선택안함
    END, // 종료
}

enum BATTLE_RESULT
{
    VICTORY,
    LOOSE,
    NONE,
}

class Charater
{
    protected string Name = "";
    protected int HP;
    protected int MAX_HP;
    protected int AT;

    static public void Attact(Charater fromCharater, Charater toCharater)
    {
        toCharater.HP = toCharater.HP - fromCharater.AT;
        toCharater.HP = toCharater.HP < 0 ? 0 : toCharater.HP;
        Console.WriteLine($"{fromCharater.Name}이(가) {toCharater.Name}을(를) 공격했습니다.");
    }
    public bool IsAlive()
    {
        return 0 < HP;
    }
    public void PrintInfo()
    {
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"HP: {HP}/{MAX_HP}");
        Console.WriteLine($"AT: {AT}");
        Console.WriteLine("-------------------------------------");
    }
}

class Monster : Charater
{
    private enum MONSTER_TYPE
    {
        EASY,
        MIDDLE,
        HARD,
    }

    MONSTER_TYPE type;
    public Monster()
    {
        type = (MONSTER_TYPE)new Random().Next(0, 3);
        InitInfo(type);
        Name = $"{type} 몬스터";
        Console.WriteLine($"{Name}가 나타났습니다.");
    }

    void InitInfo(MONSTER_TYPE type)
    {
        switch (type)
        {
            case MONSTER_TYPE.EASY:
                HP = 100;
                MAX_HP = 100;
                AT = 10;
                break;
            case MONSTER_TYPE.MIDDLE:
                HP = 150;
                MAX_HP = 150;
                AT = 15;
                break;
            case MONSTER_TYPE.HARD:
                HP = 200;
                MAX_HP = 200;
                AT = 20;
                break;
            default:
                HP = 100;
                MAX_HP = 100;
                AT = 10;
                break;
        }
    }
}

class Player : Charater
{

    public Player(string? _Name)
    {
        Name = string.IsNullOrEmpty(_Name) ? "익명" : _Name;
        HP = 120;
        MAX_HP = 120;
        AT = 15;
    }

    public void MaxHealHP()
    {
        if (HP < MAX_HP)
        {
            HP = MAX_HP;
            Console.WriteLine("체력이 회복되었습니다.");
        }
        else
        {
            Console.WriteLine("체력이 모두 회복되어 있어서 회복할 필요가 없습니다.");
        }
    }

    public void UpAT()
    {
        AT = AT + 10;
    }
}