﻿using System;
using System.Collections.Generic;

using GOG.Model;
using GOG.SharedModels;
using GOG.Interfaces;

namespace GOG.Controllers
{
    public class GameDetailsController : ProductCoreController<GameDetails>
    {
        private IDeserializeDelegate<string> stringDeserializeController;
        private IGameDetailsDownloadsController gameDetailsDownloadsController;
        private ICollection<string> supportedLanguages;

        public GameDetailsController(
            IList<GameDetails> gameDetails,
            IGetStringDelegate getStringDelegate,
            IDeserializeDelegate<string> stringDeserializeController,
            IGameDetailsDownloadsController gameDetailsDownloadsController,
            ICollection<string> supportedLanguages) :
            base(gameDetails, getStringDelegate)
        {
            this.stringDeserializeController = stringDeserializeController;
            this.gameDetailsDownloadsController = gameDetailsDownloadsController;
            this.supportedLanguages = supportedLanguages;
        }

        protected override string GetRequestTemplate()
        {
            return Urls.AccountGameDetailsTemplate;
        }

        //private void ExpandDynamicDownloads(ref GameDetails details)
        //{
        //    if (details == null) return;

        //    details.LanguageDownloads = new List<OperatingSystemsDownloads>();

        //    //foreach (var entry in details.DynamicDownloads)
        //    //{
        //    //    if (entry.Length != 2)
        //    //        throw new InvalidOperationException("Unsupported data format");

        //    //    var language = entry[0];

        //    //    if (!supportedLanguages.Contains(language)) continue;

        //    //    string downloadsString = JsonConvert.SerializeObject(entry[1]);
        //    //    var downloads = stringDeserializeController.Deserialize<OperatingSystemsDownloads>(
        //    //        downloadsString);

        //    //    downloads.Language = language;
        //    //    details.LanguageDownloads.Add(downloads);
        //    //}

        //    //details.DynamicDownloads = null;

        //    if (details.DLCs == null) return;

        //    for (var ii = 0; ii < details.DLCs.Count; ii++)
        //    {
        //        var dlc = details.DLCs[ii];
        //        ExpandDynamicDownloads(ref dlc);
        //    }
        //}

        protected override GameDetails Deserialize(string data)
        {
            // GameDetails are complicated as GOG.com currently serves mixed type array
            // where first entry is string "Language" and next entries are downloads
            // so in order to overcome this we do it like this:
            // 1) use Json.NET to deserialize with downloads mapped to dynamic[][] collection
            // 2) walk through DynamicDownloads collection
            // 3) for each language we expand second collection item (that is operating system downloads)
            // 4) for each DLC we recursively expand DynamicDownloads
            // 5) we nullify DynamicDownloads after expansion to allow further serialization

            // TODO: keep an eye on further changes that might allow to simplify this

            var downloadsString = gameDetailsDownloadsController.ExtractSingle(data);
            data = gameDetailsDownloadsController.Sanitize(data, downloadsString);

            GameDetails gameDetails = null;

            //GameDetails gameDetails = JsonConvert.DeserializeObject<GameDetails>(data);

            //ExpandDynamicDownloads(ref gameDetails);

            return gameDetails;
        }

        public override GameDetails NewDeserialize()
        {
            // Deponia has more languages
            var testData = @"{'title':'Deponia','backgroundImage':'//images-2.gog.com/e11295899365ce57254dde1ca2fb530f9b2407f37b16fa3775326e6fb2360f30','cdKey':'','textInformation':'','downloads':[['English',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/en1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_en','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/en2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__en','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/en3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__en','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['\\u010desk\\u00fd',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/cz1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_cz','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/cz2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__cz','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/cz3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__cz','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['Deutsch',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/de1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_de','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/de2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__de','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/de3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__de','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['espa\\u00f1ol',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/es1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_es','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/es2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__es','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/es3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__es','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['fran\\u00e7ais',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/fr1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_fr','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/fr2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__fr','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/fr3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__fr','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['italiano',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/it1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_it','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/it2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__it','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/it3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__it','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['polski',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/pl1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_pl','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/pl2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__pl','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/pl3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__pl','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['portugu\\u00eas',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/pt1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_pt','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/pt2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__pt','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/pt3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__pt','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['T\\u00fcrk\\u00e7e',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/tr1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_tr','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/tr2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__tr','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/tr3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__tr','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['\\u0395\\u03bb\\u03bb\\u03b7\\u03bd\\u03b9\\u03ba\\u03ac',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/gk1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_gk','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/gk2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__gk','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/gk3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__gk','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['\\u0431\\u044a\\u043b\\u0433\\u0430\\u0440\\u0441\\u043a\\u0438',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/bl1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_bl','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/bl2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__bl','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/bl3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__bl','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}],['\\u0440\\u0443\\u0441\\u0441\\u043a\\u0438\\u0439',{'windows':[{'manualUrl':'\\/downlink\\/deponia\\/ru1installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer_win_ru','name':'Deponia','version':'3.3.1357 (gog-10)','date':'','size':'1.6 GB'}],'mac':[{'manualUrl':'\\/downlink\\/deponia\\/ru2installer3','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__ru','name':'Deponia','version':'3.3.1357 (gog-11)','date':'','size':'1.6 GB'}],'linux':[{'manualUrl':'\\/downlink\\/deponia\\/ru3installer2','downloaderUrl':'gogdownloader:\\/\\/deponia\\/installer__ru','name':'Deponia','version':'3.3.1357 (gog-3)','date':'','size':'1.6 GB'}]}]],'extras':[{'manualUrl':'\\/downlink\\/file\\/deponia\\/18343','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18343','name':'manual','type':'manuals','info':1,'size':'13 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18373','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18373','name':'wallpapers','type':'wallpapers','info':9,'size':'43 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18363','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18363','name':'soundtrack (English)','type':'audio','info':1,'size':'36 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18383','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18383','name':'soundtrack (German)','type':'audio','info':1,'size':'36 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18323','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18323','name':'avatars','type':'avatars','info':25,'size':'1 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18313','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18313','name':'artworks','type':'artworks','info':24,'size':'29 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18333','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18333','name':'concept art','type':'artworks','info':1,'size':'20 MB'},{'manualUrl':'\\/downlink\\/file\\/deponia\\/18353','downloaderUrl':'gogdownloader:\\/\\/deponia\\/18353','name':'papercraft figures','type':'game add-ons','info':2,'size':'5 MB'}],'dlcs':[],'tags':[],'isPreOrder':false,'releaseTimestamp':1346878800,'messages':[],'changelog':'\\u003Ch4\\u003EPatch 3.3.1357 (18 January 2016)\\u003C\\/h4\\u003E\\n\\u003Cul\\u003E\\n\\u003Cli\\u003EEngine Update to Visionaire 4.2.5\\u003C\\/li\\u003E\\n\\u003Cli\\u003EAdded: GOG Galaxy Achievements for Windows and OS X\\u003C\\/li\\u003E\\n\\u003Cli\\u003EOSX: Fixed not being able to return to the game via the dock or with Alt-Tab\\u003C\\/li\\u003E\\n\\u003Cli\\u003EOSX: Fixed constant display of the system cursor\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Game running too fast with deactived VSync\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Disappearing characters after savegame loading\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Game crashing with activated texture compression\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Game crashing after savegame loading because of memory fragmentation\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Game crashing when using the fork with the horn at the tower\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Italian localization errors and incorrect file links \\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Localization error on menu back button \\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Several exits not reacting immediatly to clicks while standing on the destination point\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Several exits in chapter 1 not using the intended different reaction for single- and doubleclick\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Layer error when standing near the dud \\u003C\\/li\\u003E\\n\\u003Cli\\u003EAdded: Missing sound effect when picking up the wasabi peas\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Double voice of Lotti in the town hall\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Localization error when using items with the chili pepper\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Localization error when setting fire to the inventory\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Not being able to pick up the Stimulant from inventory\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Restrain Rufus while entering the Safecode\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Errors entering Toni\\u0027s shop using doubleclick from the towncenter\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Missing dialog box when entering Toni\\u0027s shop for the first time\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Missing dialog box when entering Wenzel\\u0027s house for the first time\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Hearing the walking sound of the angry Postbot in other rooms\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Rufus not taking the glas of booze while the drawer is opened\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Occasionally jumping animation when picking up the foot fetters\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Missing dialog branch with Hannek from cleaning the windshield\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Graphic layer errors in the Minebike control center resulting in missing lever images\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Animation errors when entering the elevetor code for the first time\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Localization errors while cleaning the cartridge\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Missing dialog box when talking to Goal after inserting the cleaned cartridge\\u003C\\/li\\u003E\\n\\u003Cli\\u003EFixed: Disabled exit while being cornered by Cletus\\u003C\\/li\\u003E\\n\\u003C\\/ul\\u003E','forumLink':'https:\\/\\/www.gog.com\\/forum\\/deponia_series','isBaseProductMissing':false,'missingBaseProduct':null}";

            var gd = Deserialize(testData);

            return gd;
        }
    }
}
