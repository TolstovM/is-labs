using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab5
{
    class Program
    {
		// Словарь для расшифровки (для каждого символа шифротекста - символ расшифрованного текста)
		static Dictionary<Char, Char> dict = new Dictionary<Char, Char>();
		// Частоты символов шифротекста (число повторов каждой буквы)
		static Dictionary<Char, int> char_freqs = new Dictionary<Char, int>();
		// Частоты биграмм шифротекста (пар букв)
		static Dictionary<String, int> bigram_freqs = new Dictionary<String, int>();
		// Частоты триграмм шифротекста (троек букв)
		static Dictionary<String, int> trigram_freqs = new Dictionary<String, int>();

		//Шифротекст
		//static String c_text = ("Бтышцнтнюнлптнй мбгнтнубыйлпьыън ш ьфнънеж. Уедфнс юешцйе ежтгнъ, ебхныбчкша тй ыбг, гбг аныйлньп хж — иын \n" +
		//    "рйтнъжй ьфнънеж, ын йьып оълйтшй, юеш гныненц тйьгнлпгн юенубърнъ ьфнъбешъбчыьо юнутоып рйтж шлш аныо хж тй ьтшябып ша. Бтблнфшвтжц нхебмнц ш \n" +
		//    "гнцюбтшш, гнынежй юеш тнецблптжа дьлнъшоа ьыблш хж гнтгдейтыбцш, ьфнъбешъбчыьо тй гнтгдешенъбып. Ьгбяйц, йьлш хж ъ фненуй хжлн ъьйфн уъй юйгбетш, \n" +
		//    "нтш цнфлш хж унфнънешыпьо, вынхж нутб юенубъблб ынлпгн гйгьж, б уедфбо — енфблшгш, ш цйяуд тшцш тй ънмтшглн хж гнтгдейтршш, гбг йьлш хж нхй юенубъблш ш \n" +
		//    "гйгьж ш енфблшгш. Шм-мб ьтшяйтшо гнтгдейтршш рйтж няшубйцн юнсуды ъъйеа, б юенубърж мбъйуды ежтнг ъ ыдюшг. Ъ гнтйвтнц шынфй ежтнг ъ тнецблптнц ьлдвбй \n" +
		//    "мбкшкбйы юнгдюбыйлйс шцйттн шм-мб гнтгдейтршш. Юенубърж унлятж гнтгдешенъбып, вынхж юейулбфбып хнлйй гбвйьыъйттжй ынъбеж мб лдвздч рйтд, б йьлш нтш тй \n" +
		//    "ъжуйеяшъбчы гнтгдейтршш, ын дануоы ь ежтгб. Ьнфлбзйтшй н рйтба шлш нх ныьдыьыъшш гнтгдейтршш юнуежъбйы иыд ьшьыйцд. Юештоышй цйе г тйунюдкйтшч \n" +
		//    "гнтгдейтршш — иын йкй нушт ъшу ежтнвтнфн гебаб. Ыбгшй ышюж ьнфлбзйтшс тймбгнттж ъ хнлпзштьыъй ьыебт ш юнюбубчы юну уйсьыъшй бтышцнтнюнлптнфн \n" +
		//    "мбгнтнубыйлпьыъб. Рйлп иынс федююж мбгнтнъ — тй унюдьышып тбцйейттнфн ъейуб гнтгдейтршш. Ъ ьдктньыш нтб нфебтшвшъбйы ъьй уйсьыъшо, \n" +
		//    "генцй юейуньыбълйтшо анензша ынъбенъ мб аненздч рйтд — тбюешцйе, тй унюдьгбйы ьтшяйтшо гнтгдейтршш юдыёц ьлшотшс. Бтышцнтнюнлптнй \n" +
		//    "мбгнтнубыйлпьыън ъйьпцб ьлнятн ш цж лшзп ублш йфн нхкшс тбхеньнг, тн иын йкё нушт юешцйе ынфн, гбг ежтнг цняйы юныйеюйып геба, б мбгнт цняйы \n" +
		//    "шьюнлпмнъбыпьо, вынхж иынфн тй юеншмнзлн. Гнфн иын гбьбйыьо. Хшылшрйтмшо — иын тбхне мбгнтнъ, юебъшл ш юейуюшьбтшс, гнынежй нытньоыьо г ъшеыдблптнс \n" +
		//    "ъблчый. Ъ рйлнц ъ тйс фнънешыьо, вын тдятн юнлдвшып ны уйюбеыбцйтыб юн эштбтьнъжц дьлдфбц ыбг тбмжъбйцдч «хшылшрйтмшч», йьлш ъж анышый мбтшцбыпьо \n" +
		//    "вйц-ын, вын юйейвшьлйтн тшяй. «Хшмтйь-бгышътньып ъ нхлбьыш ъшеыдблптнс ъблчыж» нмтбвбйы мбтоышй лчхжц шм юйейвшьлйттжа юенрйььнъ, ньдкйьыълойцжа ъ \n" +
		//    "Тпч-Снегй шлш яшыйлйц Тпч-Снегб: 1. юнлдвйтшй ъшеыдблптнс ъблчыж уло юйейьжлгш шлш юйейьжлгб ъшеыдблптнс ъблчыж мб шьглчвйтшйц ьлдвбйъ, гнфуб ыебтмбгршо \n" +
		//    "ньдкйьыълойыьо тй ъ эштбтьнъжа рйлоа ш тй тйьйы хнлйй вйц тнцштблптдч ьдццд ъ ъшеыдблптнс ъблчый; 2. аебтйтшй, ьнуйеябтшй шлш ньдкйьыълйтшй дюебълйтшо \n" +
		//    "шлш гнтыенло мб ъшеыдблптнс ъблчынс ны шцйтш уедфнфн лшрб; 3. юнгдюгб ш юенубяб ъшеыдблптнс ъблчыж ъ ебцгба нхьлдяшъбтшо глшйтынъ; 4. шьюнлтйтшй дьлдф \n" +
		//    "юн нхцйтд ъ ебцгба нхьлдяшъбтшо глшйтынъ; шлш 5. гнтыенлп, ебьюнеояйтшй шлш ъжюдьг ршэенънс ъблчыж. Ебмебхныгб ш ебьюеньыебтйтшй ЮН ьбцш юн ьйхй тй \n" +
		//    "оълочыьо хшмтйь-бгышътньыпч ъ нхлбьыш ъшеыдблптнс ъблчыж. Ъ ыйгьый дюнцштбчыьо «юенрйььж, ньдкйьыълойцжй ъ Тпч-Снегй шлш яшыйлйц Тпч-Снегб», вын ныебябйы \n" +
		//    "чешьушгршч уйюбеыбцйтыб. Нутбгн ълшотшй ыбгша юейуюшьбтшс ебьюеньыебтойыьо ублйгн мб юейуйлж зыбыб юн уъдц юешвштбц: ън-юйеъжа, ъ зыбыба ь хнлпзшц \n" +
		//    "тбьйлйтшйц — ыбгша гбг Тпч-Снег шлш Гблшэнетшо, фуй гнцюбтшш ъжтдяуйтж ъжхшебып цйяуд юнувштйтшйц цйьытжц мбгнтбц шлш данунц ь ежтгб ън ъьёц зыбый, \n" +
		//    "нтш юейуюнвшыбчы шьюнлтоып мбгнтж, б ън-ъынежа, тйгнынежй зыбыж ъ рйлнц ьвшыбчыьо лшуйебцш ъ нхлбьыш ейфдлшенъбтшо ныуйлптжа ьйгыненъ игнтнцшгш — эштбтьнъ \n" +
		//    "ъ Тпч-Снегй ш ыйатнлнфшс ъ Гблшэнетшш.").ToLower();

		static String c_text = ("pyt viqebov, xp q bqcvmc oxgvmzv jylof myc bvtexc tyrqocr-ptvv tvfxzctxhlcxym yp cwv btystqe hr qoo cwyzv jwy tvgvxnv gybxvz fxtvgcor yt \n" +
			"xmfxtvgcor cwtylsw ryl, cwvm cwv ymor jqr ryl gylof zqcxzpr hycw xc qmf cwxz oxgvmzv jylof hv cy tvptqxm vmcxtvor ptye fxzctxhlcxym yp cwv btystqe. xp qmr bytcxym \n" +
			"yp cwxz zvgcxym xz wvof xmnqoxf yt lmvmpytgvqhov lmfvt qmr bqtcxgloqt gxtglezcqmgv, cwv hqoqmgv yp cwv zvgcxym xz xmcvmfvf cy qbbor qmf cwv zvgcxym qz q jwyov xz \n" +
			"xmcvmfvf cy qbbor xm ycwvt gxtglezcqmgvz. xc xz myc cwv bltbyzv yp cwxz zvgcxym cy xmflgv ryl cy xmptxmsv qmr bqcvmcz yt ycwvt btybvtcr txswc goqxez yt cy gymcvzc \n" +
			"nqoxfxcr yp qmr zlgw goqxez; cwxz zvgcxym wqz cwv zyov bltbyzv yp btycvgcxms cwv xmcvstxcr yp cwv ptvv zypcjqtv fxzctxhlcxym zrzcve, jwxgw xz xebovevmcvf hr blhoxg \n" +
			"oxgvmzv btqgcxgvz.eqmr bvybov wqnv eqfv svmvtylz gymctxhlcxymz cy cwv jxfv tqmsv yp zypcjqtv fxzctxhlcvf cwtylsw cwqc zrzcve xm tvoxqmgv ym gymzxzcvmc qbboxgqcxym yp \n" +
			"cwqc zrzcve; xc xz lb cy cwv qlcwyt/fymyt cy fvgxfv xp wv yt zwv xz jxooxms cy fxzctxhlcv zypcjqtv cwtylsw qmr ycwvt zrzcve qmf q oxgvmzvv gqmmyc xebyzv cwqc gwyxgv. \n" +
			"cwxz zvgcxym xz xmcvmfvf cy eqdv cwytylswor govqt jwqc xz hvoxvnvf cy hv q gymzvalvmgv yp cwv tvzc yp cwxz oxgvmzv. xp cwv fxzctxhlcxym qmf/yt lzv yp cwv btystqe xz \n" +
			"tvzctxgcvf xm gvtcqxm gylmctxvz vxcwvt hr bqcvmcz yt hr gybrtxswcvf xmcvtpqgvz, cwv ytxsxmqo gybrtxswc wyofvt jwy boqgvz cwv btystqe lmfvt cwxz oxgvmzv eqr qff qm \n" +
			"viboxgxc svystqbwxgqo fxzctxhlcxym oxexcqcxym vigolfxms cwyzv gylmctxvz, zy cwqc fxzctxhlcxym xz bvtexccvf ymor xm yt qeyms gylmctxvz myc cwlz vigolfvf.xm zlgw gqzv, \n" +
			"cwxz oxgvmzv xmgytbytqcvz cwv oxexcqcxym qz xp jtxccvm xm cwv hyfr yp cwxz oxgvmzv. cwv ptvv zypcjqtv pylmfqcxym eqr blhoxzw tvnxzvf qmf/yt mvj nvtzxymz yp cwv svmvtqo \n" +
			"blhoxg oxgvmzv ptye cxev cy cxev.  zlgw mvj nvtzxymz jxoo hv zxexoqt xm zbxtxc cy cwv btvzvmc nvtzxym, hlc eqr fxppvt xm fvcqxo cy qfftvzz mvj btyhovez yt gymgvtmz. \n" +
			"vqgw nvtzxym xz sxnvm q fxzcxmslxzwxms nvtzxym mlehvt.  xp cwv btystqe zbvgxpxvz q nvtzxym mlehvt yp cwxz oxgvmzv jwxgw qbboxvz cy xc qmf \"qmr oqcvt nvtzxym\", \n" +
			"ryl wqnv cwv ybcxym yp pyooyjxms cwv cvtez qmf gymfxcxymz vxcwvt yp cwqc nvtzxym yt yp qmr oqcvt nvtzxym blhoxzwvf hr cwv ptvv zypcjqtv pylmfqcxym.xp cwv btystqe \n" +
			"fyvz myc zbvgxpr q nvtzxym mlehvt yp cwxz oxgvmzv, ryl eqr gwyyzv qmr nvtzxym vnvt blhoxzwvf hr cwv ptvv zypcjqtv pylmfqcxym. xp ryl jxzw cy xmgytbytqcv bqtcz yp \n" +
			"cwv btystqe xmcy ycwvt ptvv btystqez jwyzv fxzctxhlcxym gymfxcxymz qtv fxppvtvmc, jtxcv cy cwv qlcwyt cy qzd pyt bvtexzzxym.  pyt zypcjqtv jwxgw xz gybrtxswcvf hr \n" +
			"cwv ptvv zypcjqtv pylmfqcxym, jtxcv cy cwv ptvv zypcjqtv pylmfqcxym; jv zyevcxevz eqdv vigvbcxymz pyt cwxz.ylt fvgxzxym jxoo hv slxfvf hr cwv cjy syqoz yp btvzvtnxms \n" +
			"cwv ptvv zcqclz yp qoo fvtxnqcxnvz yp ylt ptvv zypcjqtv qmf yp btyeycxms cwv zwqtxms qmf tvlzv yp zypcjqtv svmvtqoor. hvgqlzv cwv btystqe xz oxgvmzvf ptvv yp gwqtsv, \n" +
			"cwvtv xz my jqttqmcr pyt cwv btystqe, cy cwv vicvmc bvtexccvf hr qbboxgqhov oqj.vigvbc jwvm ycwvtjxzv zcqcvf xm jtxcxms cwv gybrtxswc wyofvtz qmf/yt ycwvt bqtcxvz btynxfv \n" +
			"cwv btystqe \"qz xz\" jxcwylc jqttqmcr yp qmr dxmf, vxcwvt vibtvzzvf yt xeboxvf, xmgolfxms, hlc myc oxexcvf cy, cwv xeboxvf jqttqmcxvz yp evtgwqmcqhxoxcr qmf pxcmvzz pyt q \n" +
			"bqtcxgloqt bltbyzv.cwv vmcxtv txzd qz cy cwv alqoxcr qmf bvtpyteqmgv yp cwv btystqe xz jxcw ryl.zwylof cwv btystqe btynv fvpvgcxnv, ryl qzzlev cwv gyzc yp qoo mvgvzzqtr zvtnxgxms, \n" +
			"tvbqxt yt gyttvgcxym").ToLower();

		// Подсчет частот букв, биграмм и триграмм
		public static void ParseWord(String word)
		{
			// Подсчет букв
			foreach (Char letter in word)
				if (char_freqs.ContainsKey(letter))
					char_freqs[letter]++;
				else
					char_freqs.Add(letter, 1);
			// Подсчет биграмм
			for (int i = 0; i < word.Length - 1; i++)
			{
				String bigram = word.Substring(i, 2);
				if (bigram_freqs.ContainsKey(bigram))
					bigram_freqs[bigram]++;
				else
					bigram_freqs.Add(bigram, 1);
			}
			// Подсчет триграмм
			for (int i = 0; i < word.Length - 2; i++)
			{
				String trigram = word.Substring(i, 3);
				if (trigram_freqs.ContainsKey(trigram))
					trigram_freqs[trigram]++;
				else
					trigram_freqs.Add(trigram, 1);
			}
		}

		// Вывод на консоль для сопоставления слов:
		// В первой строке выводит заданный набор слов шифротекста
		// Во второй строке - соответствующие им наборы слов расшифрованного текста (по словарю dict)
		static void printHashByDict(IEnumerable<String> words)
		{
			foreach (String word in words)
				Console.Write("{0} ", word);
			Console.WriteLine();
			foreach (String word in words)
			{
				for (int i = 0; i < word.Length; i++)
					Console.Write(dict[word[i]]);
				Console.Write(" ");
			}
			Console.WriteLine();
		}

		public static void Main()
		{
			// 1. Разделяем шифротекст на слова
			String[] c_words = c_text.Split(new char[] { ',', '.', ' ', ':', '-', '\"', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			// 2. Собираем статистику по всем словам (частоты букв, пар и троек букв)
			foreach (String word in c_words)
				ParseWord(word);

			// 2.1. Сортируем буквы шифротекста по частоте и выводим на консоль
			var char_list = char_freqs.ToList();
			char_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
			Console.WriteLine("Число символов в алфавите шифротекста {0}", char_list.Count());
			foreach (var p in char_list)
				Console.WriteLine("{0} {1}", p.Key, p.Value);

			// 2.2. Сортируем биграммы шифротекста по частоте и выводим на консоль первые 5 самых частых
			int show_count = 5;
			var bigram_list = bigram_freqs.ToList();
			bigram_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
			Console.WriteLine("\nБиграммы (частые {0})", show_count);
			for (int i = 0; i < show_count; i++)
				Console.WriteLine("{0} {1}", bigram_list[i].Key, bigram_list[i].Value);

			// 2.3. Сортируем триграммы шифротекста по частоте и выводим на консоль первые 5 самых частых
			var trigtam_list = trigram_freqs.ToList();
			trigtam_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
			Console.WriteLine("\nТриграммы (частые {0})", show_count);
			for (int i = 0; i < show_count; i++)
				Console.WriteLine("{0} {1}", trigtam_list[i].Key, trigtam_list[i].Value);

			// 3. Эталонный список русских символов (по убыванию частоты)
			List<Char> rus_letters = new List<Char>() { 'о', 'е', 'а', 'и', 'н', 'т', 'с', 'р', 'в', 'л', 'к', 'м', 'д', 'п', 'у', 'я', 'ы', 'ь', 'г', 'з', 'б', 'ч', 'й', 'х', 'ж', 'ш', 'ю', 'ц', 'щ', 'э', 'ф', 'ъ' };
			// Формируем список букв шифротекста, отсортированный по убыванию частоты
			List<Char> sort_letters = char_list.Select(x => x.Key).ToList();

			// 4. Вручную заполняем словарь для расшифровки (таблица сопоставления символов шифротекста и расшифрованного текста)
			//dict.Add('ш', 'и');// удвоение
			//dict.Add('т', 'н');// удвоение
			//dict.Add('ъ', 'в');//частота + предлог

			//dict.Add('б', 'а');//частота + гбг - ото
			//dict.Add('м', 'з');// шм-мб - из-за
			//dict.Add('й', 'е');//частота + не
			//dict.Add('н', 'о');// биграма и предлоги
			//dict.Add('ь', 'с');//предлоги
			//         dict.Add('г', 'к');// биграма и предлоги
			////допущеня чтобы получить или и где
			//         dict.Add('ф', 'г');
			//dict.Add('у', 'д');
			//dict.Add('л', 'л');
			////слово законодательство
			//dict.Add('ы', 'т');
			//dict.Add('п', 'ь');
			////тпч-снегй - нью-йорке
			//dict.Add('ч', 'ю');
			//dict.Add('с', 'й');
			//dict.Add('е', 'р');

			//dict.Add('а', 'х'); // ыйатнлнфшс - технологий
			//dict.Add('э', 'ф'); // гблшэнетшш - калифорнии
			//dict.Add('х', 'б'); // нхлбьыш - области

			//dict.Add('ж', 'ы');
			//dict.Add('в', 'ч');
			//dict.Add('о', 'я');
			//dict.Add('ц', 'м');
			//dict.Add('р', 'ц');
			//dict.Add('и', 'э');
			//dict.Add('з', 'ш');
			//dict.Add('ю', 'п');
			//dict.Add('я', 'ж');
			//dict.Add('д', 'у');
			//dict.Add('ё', 'ё');
			//dict.Add('к', 'щ');


			dict.Add('v', 'e');//частота
			dict.Add('q', 'a');
			dict.Add('t', 'r');//qtv - are
			dict.Add('y', 'o');//yt - or
			dict.Add('c', 't');//частота и подозрения на предлоги
			dict.Add('m', 'n');// no, an
			dict.Add('r', 'y');//ryl - you & qmr - any
			dict.Add('f', 'd');//qmf - and
			dict.Add('l', 'u');//lmfvt - under
			dict.Add('z', 's');//lzv - use

			dict.Add('x', 'i'); // xz xmcvmfvf - is intended
			dict.Add('j', 'w'); //mvj - new & jqr - way
			dict.Add('e', 'm');// qzzlev - assume
			dict.Add('o', 'l');// jylof - would
			dict.Add('g', 'c'); //oxgvmzv - license
			dict.Add('w', 'h'); //jwy -who & has
			dict.Add('p', 'f'); //ptvv - free
			dict.Add('h', 'b');// by & redistribution 
			dict.Add('s', 'g'); //through
			dict.Add('b', 'p'); //purpose
			dict.Add('n', 'v'); //receive
			dict.Add('d', 'k'); //ask
			dict.Add('a', 'q'); //consequence
			dict.Add('i', 'x'); //extent 

			// Для символов шифротекста, для которых не подставили замену, подставляем символ '_'
			for (int i = 0; i < sort_letters.Count; i++)
				if (!dict.ContainsKey(sort_letters[i]))
					//dict.Add(sort_letters[i], rus_letters[i]);
					dict.Add(sort_letters[i], '_');

			// 5. Для проверки нашего словаря, выводим удвоения - пары из двух одинаковых букв
			// (в русском это обычно "ии", "нн", "оо", "сс")
			Console.WriteLine("\nУдвоения");        //  стр. 452
			HashSet<String> doubles = bigram_freqs.Keys.Where(s => s[0] == s[1]).ToHashSet();
			printHashByDict(doubles);
			// Выводим предлоги из 1й, 2х, 3х букв
			// Для каждого предлога из шифротекста внизу будет выведена его расшифровка по словарю dict
			Console.WriteLine("\nПредлоги");
			for (int L = 1; L <= 3; L++)
			{
				HashSet<String> short_words = c_words.Where(s => s.Length == L).ToHashSet();
				printHashByDict(short_words);
			}
			// Выводим словарь
			bool showDict = true;
			if (showDict)
			{
				Console.WriteLine("\nDictionary:");
				foreach (var p in dict)
					Console.Write("{0} ", p.Value);
				Console.Write('\n');
				foreach (var p in dict)
					Console.Write("{0} ", p.Key);
			}

			// Расшифровываем текст
			StringBuilder text_ = new StringBuilder();
			for (int i = 0; i < c_text.Length; i++)
				if (dict.ContainsKey(c_text[i]))
					text_.Append(dict[c_text[i]]);
				else
					text_.Append(c_text[i]);
			String text = text_.ToString(); // расшифрованный текст

			// Выводим строки текста и шифротекста парами - сверху строку шифротекста, под ним - строку расшифрованного текста
			String[] c_lines = c_text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			String[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			Console.WriteLine("\nТекст:");
			for (int i = 0; i < lines.Count(); i++)
			{
				//Console.WriteLine(c_lines[i]);
				Console.WriteLine("{0}\n{1}\n", c_lines[i], lines[i]);
			}
		}
	}
}
