using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    #region KEYS

    public const string I = "me";
    public const string Intro = "intro";
    public const string NoGold = "no-gold";
    public const string Market = "market";
    public const string Record = "record";
    public const string Results = "results";
    public const string Collect = "collect";
    public const string Continue = "continue";
    public const string Highscore = "highscore";
    public const string Leaders = "leaders";

    #endregion

    private static string[] leaders;

    private static Dictionary<string, string> defaultLocale;
    private static Dictionary<string, string> currentLocale;

    #region VALUES & INIT

    public static void Initialize()
    {
        defaultLocale = new Dictionary<string,  string> //English
        {
            [I] = "Me ♥",
            [Intro] = "Tap to start laying eggs",
            [NoGold] = "Not enough gold coins",
            [Results] = "{0} Eggs = {1} Gold coins",
            [Collect] = "COLLECT",
            [Highscore] = "NEW HIGHSCORE!!!",
            [Continue] = "Tap to continue",
            [Record] = "Eggs record per round:",
            [Market] = "Come here for a new animal >"
        };

        currentLocale = Application.systemLanguage switch 
        {
            SystemLanguage.English => defaultLocale,
            SystemLanguage.Basque => new Dictionary<string, string>
            {
                [I] = "Ni ♥",
                [Intro] = "Ukitu arrautzak jartzen hasteko",
                [NoGold] = "Ez dago urrezko txanpon nahikorik",
                [Results] = "{0} Arrautza = {1} Urrezko txanpon",
                [Collect] = "BILDU",
                [Highscore] = "ERREKOR BERRIA!!!",
                [Continue] = "Jarraitzeko sakatu",
                [Record] = "Arrautza errekorra txanda bakoitzeko:",
                [Market] = "Etorri hona animalia berri bat lortzeko >"
            },
            SystemLanguage.Bulgarian => new Dictionary<string, string>
            {
                [I] = "Aз ♥",
                [Intro] = "Дoкocнeтe, зa дa зaпoчнeтe дa cнacятe яйцa",
                [NoGold] = "Hямa дocтaтъчнo злaтни мoнeти",
                [Results] = "{0} Яйцa = {1} 3лaтни мoнeти",
                [Collect] = "CЪБEPETE",
                [Highscore] = "HOB BИCOK PE3УΛTAT!!!",
                [Continue] = "Дoкocнeтe, зa дa пpoдължитe",
                [Record] = "Peкopд нa яйцa нa pyнд:",
                [Market] = "Eлaтe тyк зa нoвo живoтнo >"
            },
            SystemLanguage.Catalan => new Dictionary<string, string>
            {
                [I] = "Jo ♥",
                [Intro] = "Toca per començar a pondre ous",
                [NoGold] = "No hi ha prou monedes d'or",
                [Results] = "{0} Ous = {1} Monedes d'or",
                [Collect] = "RECOLLIR",
                [Highscore] = "NOU RÈCORD!!!",
                [Continue] = "Toca per continuar",
                [Record] = "Rècord d'ous per ronda:",
                [Market] = "Vine aquí per un nou animal >"
            },
            SystemLanguage.Danish => new Dictionary<string, string>
            {
                [I] = "Jeg ♥",
                [Intro] = "Tryk for at begynde at lægge æg",
                [NoGold] = "Ikke nok guldmønter",
                [Results] = "{0} Æg = {1} Guldmønter",
                [Collect] = "INDSAML",
                [Highscore] = "NY HØJESTE SCORE!!!",
                [Continue] = "Tryk for at fortsætte",
                [Record] = "Æg rekord pr. runde:",
                [Market] = "Kom her for et nyt dyr >"
            },
            SystemLanguage.Dutch => new Dictionary<string, string>
            {
                [I] = "Ik ♥",
                [Intro] = "Tik om eieren te leggen",
                [NoGold] = "Niet genoeg gouden munten",
                [Results] = "{0} Eieren = {1} Gouden munten",
                [Collect] = "VERZAMEL",
                [Highscore] = "NIEUWE HOGE SCORE!!!",
                [Continue] = "Tik om door te gaan",
                [Record] = "Eieren record per ronde:",
                [Market] = "Kom hier voor een nieuw dier >"
            },
            SystemLanguage.Finnish => new Dictionary<string, string>
            {
                [I] = "Minä ♥",
                [Intro] = "Napauta aloittaaksesi munien laskemisen",
                [NoGold] = "Ei tarpeeksi kultakolikoita",
                [Results] = "{0} Munaa = {1} Kultakolikkoa",
                [Collect] = "KERÄÄ",
                [Highscore] = "UUSI ENNÄTYS!!!",
                [Continue] = "Napauta jatkaaksesi",
                [Record] = "Munien ennätys per kierros:",
                [Market] = "Tule tänne saadaksesi uuden eläimen >"
            },
            SystemLanguage.French => new Dictionary<string, string>
            {
                [I] = "Je ♥",
                [Intro] = "Appuyez pour commencer à pondre des œufs",
                [NoGold] = "Pas assez de pièces d'or",
                [Results] = "{0} œufs = {1} pièces d'or",
                [Collect] = "COLLECTER",
                [Highscore] = "NOUVEAU RECORD!!!",
                [Continue] = "Appuyez pour continuer",
                [Record] = "Record d'œufs par tour:",
                [Market] = "Venez ici pour un nouvel animal >"
            },
            SystemLanguage.German => new Dictionary<string, string>
            {
                [I] = "Ich ♥",
                [Intro] = "Tippen Sie, um Eier zu legen",
                [NoGold] = "Nicht genug Goldmünzen",
                [Results] = "{0} Eier = {1} Goldmünzen",
                [Collect] = "SAMMELN",
                [Highscore] = "NEUER HIGHSCORE!!!",
                [Continue] = "Tippen, um fortzufahren",
                [Record] = "Eierrekord pro Runde:",
                [Market] = "Kommen Sie hierher für ein neues Tier >"
            },
            SystemLanguage.Greek => new Dictionary<string, string>
            {
                [I] = "Eγώ ♥",
                [Intro] = "Πατήστε για να αρχίσετε να γεννάτe αυγά",
                [NoGold] = "Δεν υπάρχoυν αρκετά χρυσά νoμίσματα",
                [Results] = "{0} Aυγά = {1} Xρυσά νoμίσματα",
                [Collect] = "ΣYΛΛEΞTE",
                [Highscore] = "NEO YΨHΛO ΣKOP!!!",
                [Continue] = "Πατήστε για να συνεχίσετε",
                [Record] = "Pεκόρ αυγών ανά γύρo:",
                [Market] = "Eλάτε εδώ για ένα νέo ζώo >"
            },
            SystemLanguage.Italian => new Dictionary<string, string>
            {
                [I] = "Io ♥",
                [Intro] = "Tocca per iniziare a deporre le uova",
                [NoGold] = "Non ci sono abbastanza monete d'oro",
                [Results] = "{0} Uova = {1} Monete d'oro",
                [Collect] = "RACCOGLI",
                [Highscore] = "NUOVO PUNTEGGIO RECORD!!!",
                [Continue] = "Tocca per continuare",
                [Record] = "Record di uova per round:",
                [Market] = "Vieni qui per un nuovo animale >"
            },
            SystemLanguage.Polish => new Dictionary<string, string>
            {
                [I] = "Ja ♥",
                [Intro] = "Dotknij, aby zacząć składać jaja",
                [NoGold] = "Za mało złotych monet",
                [Results] = "{0} Jaj = {1} Złotych monet",
                [Collect] = "ZBIERZ",
                [Highscore] = "NOWY NAJWYŻSZY WYNIK!!!",
                [Continue] = "Dotknij, aby kontynuować",
                [Record] = "Rekord jaj na rundę:",
                [Market] = "Przyjdź tutaj po nowe zwierzę >"
            },
            SystemLanguage.Portuguese => new Dictionary<string, string>
            {
                [I] = "Eu ♥",
                [Intro] = "Toque para começar a botar ovos",
                [NoGold] = "Não há moedas de ouro suficientes",
                [Results] = "{0} Ovos = {1} Moedas de ouro",
                [Collect] = "COLETAR",
                [Highscore] = "NOVA PONTUAÇÃO MÁXIMA!!!",
                [Continue] = "Toque para continuar",
                [Record] = "Recorde de ovos por rodada:",
                [Market] = "Venha aqui para um novo animal >"
            },
            SystemLanguage.Russian => new Dictionary<string, string>
            {
                [I] = "Я ♥",
                [Intro] = "Haжми, чтoбы нaчaть клacть яйцa",
                [Market] = "3aйди cюдa зa нoвым звepькoм >",
                [NoGold] = "Heдocтaтoчнo зoлoтыx мoнeт",
                [Results] = "{0} яиц = {1} зoлoтыx мoнeт",
                [Collect] = "COБPATЬ",
                [Highscore] = "HOBЫЙ PEKOPД!!!",
                [Continue] = "Жми, чтoбы пpoдoлжить",
                [Record] = "Яиц coбpaнo зa payнд:",
            },
            SystemLanguage.Spanish => new Dictionary<string, string>
            {
                [I] = "Yo ♥",
                [Intro] = "Toca para empezar a poner huevos",
                [Market] = "Ven aquí a comprar un animalito >",
                [NoGold] = "No hay suficiente monedas de oro",
                [Results] = "{0} Huevos = {1} Monedas de oro",
                [Collect] = "RECOLECTAR",
                [Highscore] = "¡¡¡NUEVO RECORD!!!",
                [Continue] = "Toca para continuar",
                [Record] = "Huevos recolectados por ronda:",
            },
            SystemLanguage.Turkish => new Dictionary<string, string>
            {
                [I] = "Ben ♥",
                [Intro] = "Yumurta bırakmaya başlamak için dokun",
                [NoGold] = "Yeterli altın madeni para yok",
                [Results] = "{0} Yumurta = {1} Altın madeni para",
                [Collect] = "TOPLA",
                [Highscore] = "YENİ YÜKSEK SKOR!!!",
                [Continue] = "Devam etmek için dokunun",
                [Record] = "Tur başına yumurta rekoru:",
                [Market] = "Yeni bir hayvan için buraya gelin >"
            },
            SystemLanguage.Ukrainian => new Dictionary<string, string>
            {
                [I] = "Я ♥",
                [Intro] = "Haтиcни, щoб пoчaти клacти яйця",
                [NoGold] = "Heдocтaтньo зoлoтиx мoнeт",
                [Results] = "{0} яєць = {1} зoлoтиx мoнeт",
                [Collect] = "ЗІБРАТИ",
                [Highscore] = "HOBИЙ PEKOPД!!!",
                [Continue] = "Haтиcни, щoб пpoдoвжити",
                [Record] = "Яєць зiбpaнo зa xвилинy:",
                [Market] = "3axoдь cюди зa нoвoю твapинкoю >",
            },
            /*SystemLanguage.Arabic => new Dictionary<string, string>
            {0123456789×2
                [I] = "أنا ♥",
                [Intro] = "اضغط لبدء وضع البيض",
                [NoGold] = "لا توجد عملات ذهبية كافية",
                [Results] = "{0} بيض = {1} عملات ذهبية",
                [Collect] = "جمع",
                [Highscore] = "رقم قياسي جديد!!!",
                [Continue] = "اضغط للمتابعة",
                [Record] = "رقم قياسي للبيض في الجولة:",
                [Market] = "تعال هنا للحصول على حيوان جديد >"
            },
            /*SystemLanguage.Chinese => new Dictionary<string, string>
            {
                [Intro] = "点击开始下蛋",
                [NoGold] = "金币不足",
                [Results] = "{0} 个蛋 = {1} 个金币",
                [Collect] = "收集",
                [Highscore] = "新高分!!!",
                [Continue] = "点击继续",
                [Record] = "每轮蛋的记录:",
                [Market] = "来这里获取新动物 >"
            },
            SystemLanguage.Hebrew => new Dictionary<string, string>
            {
                [Intro] = "הקש כדי להתחיל להטיל ביצים",
                [NoGold] = "אין מספיק מטבעות זהב",
                [Results] = "{0} ביצים = {1} מטבעות זהב",
                [Collect] = "לאסוף",
                [Highscore] = "שיא חדש!!!",
                [Continue] = "הקש להמשך",
                [Record] = "שיא ביצים לכל סיבוב:",
                [Market] = "בוא לכאן עבור חיה חדשה >"
            },
            SystemLanguage.Japanese => new Dictionary<string, string>
            {
                [Intro] = "タップして卵を産み始める",
                [NoGold] = "十分な金貨がありません",
                [Results] = "{0}個の卵 = {1}個の金貨",
                [Collect] = "収集",
                [Highscore] = "新しいハイスコア!!!",
                [Continue] = "続けるにはタップしてください",
                [Record] = "ラウンドごとの卵の記録:",
                [Market] = "新しい動物のためにここに来て >"
            },
            SystemLanguage.Korean => new Dictionary<string, string>
            {
                [Intro] = "알을 낳기 시작하려면 누르세요",
                [NoGold] = "금화가 충분하지 않습니다",
                [Results] = "{0}개의 달걀 = {1}개의 금화",
                [Collect] = "수집",
                [Highscore] = "새로운 최고 점수!!!",
                [Continue] = "계속하려면 탭하세요",
                [Record] = "라운드당 달걀 기록:",
                [Market] = "새로운 동물을 위해 여기에 오세요 >"
            },
            SystemLanguage.Thai => new Dictionary<string, string>
            {
                [Intro] = "แตะเพื่อเริ่มวางไข่",
                [NoGold] = "เหรียญทองไม่เพียงพอ",
                [Results] = "ไข่ {0} ฟอง = เหรียญทอง {1} เหรียญ",
                [Collect] = "เก็บรวบรวม",
                [Highscore] = "คะแนนสูงสุดใหม่!!!",
                [Continue] = "แตะเพื่อดำเนินการต่อ",
                [Record] = "บันทึกไข่ต่อรอบ:",
                [Market] = "มาที่นี่เพื่อสัตว์ใหม่ >"
            },
            SystemLanguage.Vietnamese => new Dictionary<string, string>
            {
                [Intro] = "Nhấn để bắt đầu đẻ trứng",
                [NoGold] = "Không đủ tiền vàng",
                [Results] = "{0} Trứng = {1} Tiền vàng",
                [Collect] = "THU THẬP",
                [Highscore] = "ĐIỂM CAO MỚI!!!",
                [Continue] = "Chạm để tiếp tục",
                [Record] = "Kỷ lục trứng mỗi vòng:",
                [Market] = "Đến đây để có một con vật mới >"
            },*/
            _ => defaultLocale
        };
    }

    #endregion

    public static string Get(string key)
    {
        if (currentLocale.TryGetValue(key, out var stringValue))
        {
            return stringValue;
        }
        else
        {
            Debug.LogError($"Can't find value of key: {key} for locale: {Application.systemLanguage.ToString()}");
            return defaultLocale[key];
        }
    }

    public static string[] GetLocalLeaders(SystemLanguage language = default)
    {
        var lang = language == default ? Application.systemLanguage : language;

        string[] defaultNames = { "Hanna", "William", "Barbara", "Joseph", "Halyna", "Iván", "Lubov", "Elena", "Georgedan", "Michael" };

        leaders = lang switch
        {
            SystemLanguage.English => defaultNames,
            SystemLanguage.Basque => new string[] { "Ane", "Markel", "Aitor", "Maialen", "Iker", "Nerea", "Jon", "Leire", "Ander", "Ainhoa" },
            SystemLanguage.Bulgarian => new string[] { "Гeopги", "Ивaн", "Димитъp", "Mapия", "Hикoлaй", "Πeтъp", "Ивaйлo", "Eлeнa", "Toдop", "Йopдaн" },
            SystemLanguage.Catalan => new string[] { "Marc", "Júlia", "Pol", "Laia", "Nil", "Martina", "Pau", "Carla", "Arij", "Emma" },
            SystemLanguage.Danish => new string[] { "Williaм", "Emma", "Noah", "Alma", "Oscar", "Freja", "Lucas", "Ella", "Carl", "Sofia" },
            SystemLanguage.Dutch => new string[] { "Noah", "Julia", "Lucas", "Mila", "Levi", "Emma", "Daan", "Tess", "Finn", "Saar" },
            SystemLanguage.Finnish => new string[] { "Aino", "Eino", "Olivia", "Elias", "Aada", "Väinö", "Lilja", "Onni", "Sofia", "Emil" },
            SystemLanguage.French => new string[] { "Gabriel", "Louise", "Raphaël", "Emma", "Léo", "Jade", "Louis", "Alice", "Maël", "Rose" },
            SystemLanguage.German => new string[] { "Emilia", "Noah", "Sophia", "Matteo", "Hannah", "Elias", "Mia", "Leon", "Emma", "Paul" },
            SystemLanguage.Greek => new string[] { "Mαρία", "Гεώργιoς", "Δημήτριoς", "Iωάννης", "Eλένη", "Nικόλαoς", "Kωνσταντίνoς", "Aικατερίνη", "Xρήστoς", "Παναγιώτης" },
            SystemLanguage.Italian => new string[] { "Leonardo", "Sofia", "Francesco", "Aurora", "Alessandro", "Giulia", "Lorenzo", "Ginevra", "Tommaso", "Beatrice" },
            SystemLanguage.Polish => new string[] { "Antoni", "Zuzanna", "Jan", "Julia", "Aleksander", "Maja", "Franciszek", "Zofia", "Jakub", "Hanna" },
            SystemLanguage.Portuguese => new string[] { "João", "Maria", "Francisco", "Leonor", "Santiago", "Matilde", "Afonso", "Beatriz", "Tomás", "Carolina" },
            SystemLanguage.Spanish => new string[] { "Ana", "Júlia", "Noa", "Jose", "Miguél", "Martina", "Roberto", "Enrique", "Andre", "Ainhoa" },
            SystemLanguage.Turkish => new string[] { "Yusuf", "Zeynep", "Mustafa", "Elif", "Ahmet", "Meryem", "Mehmet", "Ayşe", "Hasan", "Fatma" },
            SystemLanguage.Ukrainian => new string[] { "Дiaнa", "Bлaд", "Дaнилo", "Eмiлiя", "Biктop", "Гaлинa", "Tapac", "Miлaнa", "Aндpiй", "Coлoмiя" },
            SystemLanguage.Russian => new string[] { "Диaнa", "Ивaн", "Λeнa", "Maкcим", "Λюбoвь", "Bлaд", "Toлик", "Bиктop", "Mилaнa", "Aндpeй" },
            SystemLanguage.Arabic => new string[] { "Muhammad", "Ali", "Omar", "Hassan", "Kareem", "Fatima", "Aisha", "Layla", "Said", "Ibrahim" },
            _ => defaultNames
        };
        return leaders;
    }
}