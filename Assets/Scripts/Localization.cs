using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    #region KEYS

    public const string Intro = "intro";
    public const string NoGold = "no-gold";
    public const string Results = "results";
    public const string Collect = "collect";

    #endregion

    private static Dictionary<string, string> defaultLocale;
    private static Dictionary<string, string> currentLocale;

    #region VALUES & INIT

    public static void Initialize()
    {
        defaultLocale = new Dictionary<string,  string> //English
        {
            [Intro] = "Tap to start laying eggs",
            [NoGold] = "Not enough gold coins",
            [Results] = "{0} Eggs = {1} Gold coins",
            [Collect] = "COLLECT"
        };

        currentLocale = Application.systemLanguage switch 
        {
            SystemLanguage.English => defaultLocale,
            SystemLanguage.Arabic => new Dictionary<string,  string>
            {
                [Intro] = "اضغط لبدء وضع البيض",
                [NoGold] = "لا توجد عملات ذهبية كافية",
                [Results] = "{0} بيضة = {1} عملة ذهبية",
                [Collect] = "جمع"
            },
            SystemLanguage.Spanish => new Dictionary<string,  string>
            {
                [Intro] = "Toca para empezar a poner huevos",
                [NoGold] = "No hay suficiente monedas de oro",
                [Results] = "{0} Huevos = {1} Monedas de oro",
                [Collect] = "RECOLECTAR"
            },
            SystemLanguage.French => new Dictionary<string,  string>
            {
                [Intro] = "Appuyez pour commencer à pondre des œufs",
                [NoGold] = "Pas assez de pièces d'or",
                [Results] = "{0} Œufs = {1} Pièces d'or",
                [Collect] = "COLLECTER"
            },
            SystemLanguage.German => new Dictionary<string,  string>
            {
                [Intro] = "Tippe, um mit dem Eierlegen zu beginnen",
                [NoGold] = "Nicht genug Goldmünzen",
                [Results] = "{0} Eier = {1} Goldmünzen",
                [Collect] = "SAMMELN"
            },
            SystemLanguage.Ukrainian => new Dictionary<string,  string>
            {
                [Intro] = "Натисніть, щоб почати класти яйця",
                [NoGold] = "Недостатньо золотих монет",
                [Results] = "{0} яєць = {1} золотих монет",
                [Collect] = "ЗІБРАТИ"
            },
            SystemLanguage.Russian => new Dictionary<string,  string>
            {
                [Intro] = "Нажмите, чтобы начать класть яйца",
                [NoGold] = "Недостаточно золотых монет",
                [Results] = "{0} яиц = {1} золотых монет",
                [Collect] = "СОБРАТЬ"
            },
            SystemLanguage.Chinese => new Dictionary<string,  string>
            {
                [Intro] = "点击开始下蛋",
                [NoGold] = "没有足够的金币",
                [Results] = "{0} 个鸡蛋 = {1} 个金币",
                [Collect] = "收集"
            },
            SystemLanguage.Japanese => new Dictionary<string,  string>
            {
                [Intro] = "タップして卵を産み始める",
                [NoGold] = "十分な金貨がありません",
                [Results] = "{0}個の卵 = {1}個の金貨",
                [Collect] = "収集"
            },
            SystemLanguage.Italian => new Dictionary<string, string>
            {
                [Intro] = "Tocca per iniziare a deporre le uova",
                [NoGold] = "Non ci sono abbastanza monete d'oro",
                [Results] = "{0} Uova = {1} Monete d'oro",
                [Collect] = "RACCOGLI",
            },
            SystemLanguage.Basque => new Dictionary<string, string>
            {
                [Intro] = "Ukitu arrautzak jartzen hasteko",
                [NoGold] = "Ez dago urrezko txanpon nahikorik",
                [Results] = "{0} Arrautza = {1} Urrezko txanpon",
                [Collect] = "BILDU",
            },
            SystemLanguage.Catalan => new Dictionary<string, string>
            {
                [Intro] = "Toca per començar a pondre ous",
                [NoGold] = "No hi ha prou monedes d'or",
                [Results] = "{0} Ous = {1} Monedes d'or",
                [Collect] = "RECOLLIR",
            },
            SystemLanguage.Korean => new Dictionary<string, string>
            {
                [Intro] = "알을 낳기 시작하려면 누르세요",
                [NoGold] = "금화가 충분하지 않습니다",
                [Results] = "{0}개의 달걀 = {1}개의 금화",
                [Collect] = "수집",
            },
            SystemLanguage.Greek => new Dictionary<string, string>
            {
                [Intro] = "Πατήστε για να αρχίσετε να γεννάτε αυγά",
                [NoGold] = "Δεν υπάρχουν αρκετά χρυσά νομίσματα",
                [Results] = "{0} Αυγά = {1} Χρυσά νομίσματα",
                [Collect] = "ΣΥΛΛΕΞΤΕ",
            },
            SystemLanguage.Hebrew => new Dictionary<string, string>
            {
                [Intro] = "הקש כדי להתחיל להטיל ביצים",
                [NoGold] = "אין מספיק מטבעות זהב",
                [Results] = "{0} ביצים = {1} מטבעות זהב",
                [Collect] = "לאסוף",
            },
            SystemLanguage.Thai => new Dictionary<string, string>
            {
                [Intro] = "แตะเพื่อเริ่มวางไข่",
                [NoGold] = "เหรียญทองไม่เพียงพอ",
                [Results] = "ไข่ {0} ฟอง = เหรียญทอง {1} เหรียญ",
                [Collect] = "เก็บรวบรวม",
            },
            SystemLanguage.Vietnamese => new Dictionary<string, string>
            {
                [Intro] = "Nhấn để bắt đầu đẻ trứng",
                [NoGold] = "Không đủ tiền vàng",
                [Results] = "{0} Trứng = {1} Tiền vàng",
                [Collect] = "THU THẬP",
            },
            SystemLanguage.Danish => new Dictionary<string, string>
            {
                [Intro] = "Tryk for at begynde at lægge æg",
                [NoGold] = "Ikke nok guldmønter",
                [Results] = "{0} Æg = {1} Guldmønter",
                [Collect] = "INDSAML",
            },
            SystemLanguage.Portuguese => new Dictionary<string, string>
            {
                [Intro] = "Toque para começar a botar ovos",
                [NoGold] = "Não há moedas de ouro suficientes",
                [Results] = "{0} Ovos = {1} Moedas de ouro",
                [Collect] = "COLETAR",
            },
            SystemLanguage.Dutch => new Dictionary<string, string>
            {
                [Intro] = "Tik om eieren te leggen",
                [NoGold] = "Niet genoeg gouden munten",
                [Results] = "{0} Eieren = {1} Gouden munten",
                [Collect] = "VERZAMEL",
            },
            SystemLanguage.Bulgarian => new Dictionary<string, string>
            {
                [Intro] = "Докоснете, за да започнете да снасяте яйца",
                [NoGold] = "Няма достатъчно златни монети",
                [Results] = "{0} Яйца = {1} Златни монети",
                [Collect] = "СЪБЕРЕТЕ",
            },
            SystemLanguage.Polish => new Dictionary<string, string>
            {
                [Intro] = "Dotknij, aby zacząć składać jaja",
                [NoGold] = "Za mało złotych monet",
                [Results] = "{0} Jaj = {1} Złotych monet",
                [Collect] = "ZBIERZ",
            },
            SystemLanguage.Finnish => new Dictionary<string, string>
            {
                [Intro] = "Napauta aloittaaksesi munien laskemisen",
                [NoGold] = "Ei tarpeeksi kultakolikoita",
                [Results] = "{0} Munaa = {1} Kultakolikkoa",
                [Collect] = "KERÄÄ",
            },
            SystemLanguage.Turkish => new Dictionary<string, string>
            {
                [Intro] = "Yumurta bırakmaya başlamak için dokun",
                [NoGold] = "Yeterli altın madeni para yok",
                [Results] = "{0} Yumurta = {1} Altın madeni para",
                [Collect] = "TOPLA",
            },
            _ => defaultLocale
        };
    }

    #endregion

    public static string Get(string key)
    {
        if (currentLocale.ContainsKey(key))
            return currentLocale[key];
        else
            return defaultLocale[key];
    }
}