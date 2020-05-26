using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LOLFreeView
{
    public partial class Form1 : Form
    {
        string api_key = "RGAPI-7580c8b0-de3b-4659-b283-8632fd2052c7";
        string profileiconV = "";
        string championV = "";

        string accountId1 = "";
        string accountId2 = "";
        string accountId3 = "";
        string accountId4 = "";
        string accountId5 = "";

        public void LOLState()
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://ddragon.leagueoflegends.com/realms/kr.json";
                string json_summonerInfo = client.DownloadString(url);
                LOLState summoner = JsonConvert.DeserializeObject<LOLState>(json_summonerInfo);
                profileiconV = summoner.n.profileicon;
                championV = summoner.n.champion;

            }
        }

        public void SummonerInfo(string Name, int Num)//소환사 기본 정보 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + Name + "?api_key=" + api_key;
                string json_summonerInfo = client.DownloadString(url);
                SummonerInfo summoner = JsonConvert.DeserializeObject<SummonerInfo>(json_summonerInfo);
                string profileIconId = Convert.ToString(summoner.profileIconId);//소환사 아이콘 표기 시작
                string url_img = "http://ddragon.leagueoflegends.com/cdn/" + profileiconV + "/img/profileicon/" + profileIconId + ".png";

                switch (Num)
                {
                    case 0:
                        label11.Text = summoner.name;//소환사 이름 표기
                        pictureBox1.Load(url_img);//소환사 아이콘 표기 끝
                        accountId1 = summoner.accountId;
                        break;
                    case 1:
                        label22.Text = summoner.name;//소환사 이름 표기
                        pictureBox3.Load(url_img);//소환사 아이콘 표기 끝
                        accountId2 = summoner.accountId;
                        break;
                    case 2:
                        label44.Text = summoner.name;//소환사 이름 표기
                        pictureBox5.Load(url_img);//소환사 아이콘 표기 끝
                        accountId3 = summoner.accountId;
                        break;
                    case 3:
                        label39.Text = summoner.name;//소환사 이름 표기
                        pictureBox7.Load(url_img);//소환사 아이콘 표기 끝
                        accountId4 = summoner.accountId;
                        break;
                    case 4:
                        label55.Text = summoner.name;//소환사 이름 표기
                        pictureBox9.Load(url_img);//소환사 아이콘 표기 끝
                        accountId5 = summoner.accountId;
                        break;
                }
                

                SummonerRank(summoner.id, Num);//소환사 더 자세한 정보 표시 함수로 이동
                
                SummonerMatch(summoner.accountId, Num);
            }
        }

        public void SummonerRank(string Id, int Num) //소환사 자세한 정보 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/league/v4/positions/by-summoner/" + Id + "?api_key=" + api_key;
                string json_summonerRank = client.DownloadString(url);
                var Rank = JsonConvert.DeserializeObject<List<Example>>(@json_summonerRank);

                foreach (var inner in Rank)//소환사 자세한 정보 관련 표기
                {
                    switch (Num)
                    {
                        case 0:
                            label10.Text = inner.tier + " " + inner.rank;
                            label9.Text = "(" + Convert.ToString(inner.leaguePoints) + "LP)";
                            break;
                        case 1:
                            label21.Text = inner.tier + " " + inner.rank;
                            label20.Text = "(" + Convert.ToString(inner.leaguePoints) + "LP)";
                            break;
                        case 2:
                            label43.Text = inner.tier + " " + inner.rank;
                            label42.Text = "(" + Convert.ToString(inner.leaguePoints) + "LP)";
                            break;
                        case 3:
                            label38.Text = inner.tier + " " + inner.rank;
                            label37.Text = "(" + Convert.ToString(inner.leaguePoints) + "LP)";
                            break;
                        case 4:
                            label54.Text = inner.tier + " " + inner.rank;
                            label53.Text = "(" + Convert.ToString(inner.leaguePoints) + "LP)";
                            break;
                    }
                    
                    
                }
            }
        }

        float kills = 0;
        float deaths = 0;
        float assists = 0;
        float cs = 0;
        float win = 0;
        float loss = 0;
        int winning = 0;
        bool winning_check = true;
        public int SummonerKda(string GameId, string accountid, int Num) //소환사 KDA, CS 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/match/v4/matches/" + GameId + "?api_key=" + api_key;
                string json_summonerKda = client.DownloadString(url);
                SummonerKda summoner = JsonConvert.DeserializeObject<SummonerKda>(@json_summonerKda);
                List<ParticipantIdentity> participantIdentities = summoner.participantIdentities;
                List<Participant> participants = summoner.participants;
                if(summoner.gameDuration < 300)
                {
                    return 1;
                }
                int ans = 0;
                for(int i = 0; i < 10; i++)
                {
                    if(participantIdentities[i].player.accountId == accountid)
                    {
                        ans = i;
                        break;
                    }
                }

                if (participants[ans].stats.win == true)
                {
                    win++;
                }
                else
                {
                    loss++;
                }

                if(participants[ans].stats.win == true && winning_check == true)//연승 확인
                {
                    winning += 1;
                }
                else
                {
                    winning_check = false;
                }

                kills += participants[ans].stats.kills;
                deaths += participants[ans].stats.deaths;
                assists += participants[ans].stats.assists;
                cs += participants[ans].stats.totalMinionsKilled + participants[ans].stats.neutralMinionsKilled;

            }
            return 0;
        }

        public void SummonerMatch(string accountId, int Num)//소환사 기본 정보 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/match/v4/matchlists/by-account/" + accountId + "?api_key=" + api_key;
                string json_summonerInfo = client.DownloadString(url);
                SummonerMatch summoner = JsonConvert.DeserializeObject<SummonerMatch>(json_summonerInfo);
                List<Match> matches = summoner.matches;
                int m = 20;
                for(int i = 0; i < m; i++)
                {
                    if(i == 10)
                        progressBar1.Value += 10;
                    if (matches[i].queue == 420)
                    {
                        string matchId = Convert.ToString(matches[i].gameId);
                        if(SummonerKda(matchId, accountId, Num) == 1)
                        {
                            m++;
                        }
                    }
                    else
                    {
                        m++;
                    }
                }
                float rate = win / (win + loss) * 100;
                float kda = (kills + assists) / deaths;
                switch (Num)
                {
                    case 0:
                        label8.Text = rate.ToString("N0") + "%";
                        label7.Text = kda.ToString("N2") + ":1";
                        label6.Text = (cs / 20).ToString("N1") + " CS";
                        label5.Text = Convert.ToString(winning) + " 연승중";
                        break;
                    case 1:
                        label19.Text = rate.ToString("N0") + "%";
                        label18.Text = kda.ToString("N2") + ":1";
                        label17.Text = (cs / 20).ToString("N1") + " CS";
                        label16.Text = Convert.ToString(winning) + " 연승중";
                        break;
                    case 2:
                        label41.Text = rate.ToString("N0") + "%";
                        label40.Text = kda.ToString("N2") + ":1";
                        label28.Text = (cs / 20).ToString("N1") + " CS";
                        label27.Text = Convert.ToString(winning) + " 연승중";
                        break;
                    case 3:
                        label36.Text = rate.ToString("N0") + "%";
                        label35.Text = kda.ToString("N2") + ":1";
                        label34.Text = (cs / 20).ToString("N1") + " CS";
                        label33.Text = Convert.ToString(winning) + " 연승중";
                        break;
                    case 4:
                        label52.Text = rate.ToString("N0") + "%";
                        label51.Text = kda.ToString("N2") + ":1";
                        label50.Text = (cs / 20).ToString("N1") + " CS";
                        label49.Text = Convert.ToString(winning) + " 연승중";
                        break;
                }
                
                
            }
            
        }

        void LOLChamp(string champ, int Num)
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "http://ddragon.leagueoflegends.com/cdn/" + championV + "/data/ko_KR/champion.json";
                string json_summonerInfo = client.DownloadString(url);
                LOLChamp summoner = JsonConvert.DeserializeObject<LOLChamp>(json_summonerInfo);
                string key = "";
                if (champ == summoner.data.Aatrox.name) key = summoner.data.Aatrox.key; if (champ == summoner.data.Ahri.name) key = summoner.data.Ahri.key; if (champ == summoner.data.Akali.name) key = summoner.data.Akali.key; if (champ == summoner.data.Alistar.name) key = summoner.data.Alistar.key; if (champ == summoner.data.Amumu.name) key = summoner.data.Amumu.key; if (champ == summoner.data.Anivia.name) key = summoner.data.Anivia.key; if (champ == summoner.data.Annie.name) key = summoner.data.Annie.key; if (champ == summoner.data.Ashe.name) key = summoner.data.Ashe.key; if (champ == summoner.data.AurelionSol.name) key = summoner.data.AurelionSol.key; if (champ == summoner.data.Azir.name) key = summoner.data.Azir.key; if (champ == summoner.data.Bard.name) key = summoner.data.Bard.key; if (champ == summoner.data.Blitzcrank.name) key = summoner.data.Blitzcrank.key; if (champ == summoner.data.Brand.name) key = summoner.data.Brand.key; if (champ == summoner.data.Braum.name) key = summoner.data.Braum.key; if (champ == summoner.data.Caitlyn.name) key = summoner.data.Caitlyn.key; if (champ == summoner.data.Camille.name) key = summoner.data.Camille.key; if (champ == summoner.data.Cassiopeia.name) key = summoner.data.Cassiopeia.key; if (champ == summoner.data.Chogath.name) key = summoner.data.Chogath.key; if (champ == summoner.data.Corki.name) key = summoner.data.Corki.key; if (champ == summoner.data.Darius.name) key = summoner.data.Darius.key; if (champ == summoner.data.Diana.name) key = summoner.data.Diana.key; if (champ == summoner.data.DrMundo.name) key = summoner.data.DrMundo.key; if (champ == summoner.data.Draven.name) key = summoner.data.Draven.key; if (champ == summoner.data.Ekko.name) key = summoner.data.Ekko.key; if (champ == summoner.data.Elise.name) key = summoner.data.Elise.key; if (champ == summoner.data.Evelynn.name) key = summoner.data.Evelynn.key; if (champ == summoner.data.Ezreal.name) key = summoner.data.Ezreal.key; if (champ == summoner.data.Fiddlesticks.name) key = summoner.data.Fiddlesticks.key; if (champ == summoner.data.Fiora.name) key = summoner.data.Fiora.key; if (champ == summoner.data.Fizz.name) key = summoner.data.Fizz.key; if (champ == summoner.data.Galio.name) key = summoner.data.Galio.key; if (champ == summoner.data.Gangplank.name) key = summoner.data.Gangplank.key; if (champ == summoner.data.Garen.name) key = summoner.data.Garen.key; if (champ == summoner.data.Gnar.name) key = summoner.data.Gnar.key; if (champ == summoner.data.Gragas.name) key = summoner.data.Gragas.key; if (champ == summoner.data.Graves.name) key = summoner.data.Graves.key; if (champ == summoner.data.Hecarim.name) key = summoner.data.Hecarim.key; if (champ == summoner.data.Heimerdinger.name) key = summoner.data.Heimerdinger.key; if (champ == summoner.data.Illaoi.name) key = summoner.data.Illaoi.key; if (champ == summoner.data.Irelia.name) key = summoner.data.Irelia.key; if (champ == summoner.data.Ivern.name) key = summoner.data.Ivern.key; if (champ == summoner.data.Janna.name) key = summoner.data.Janna.key; if (champ == summoner.data.JarvanIV.name) key = summoner.data.JarvanIV.key; if (champ == summoner.data.Jax.name) key = summoner.data.Jax.key; if (champ == summoner.data.Jayce.name) key = summoner.data.Jayce.key; if (champ == summoner.data.Jhin.name) key = summoner.data.Jhin.key; if (champ == summoner.data.Jinx.name) key = summoner.data.Jinx.key; if (champ == summoner.data.Kaisa.name) key = summoner.data.Kaisa.key; if (champ == summoner.data.Kalista.name) key = summoner.data.Kalista.key; if (champ == summoner.data.Karma.name) key = summoner.data.Karma.key; if (champ == summoner.data.Karthus.name) key = summoner.data.Karthus.key; if (champ == summoner.data.Kassadin.name) key = summoner.data.Kassadin.key; if (champ == summoner.data.Katarina.name) key = summoner.data.Katarina.key; if (champ == summoner.data.Kayle.name) key = summoner.data.Kayle.key; if (champ == summoner.data.Kayn.name) key = summoner.data.Kayn.key; if (champ == summoner.data.Kennen.name) key = summoner.data.Kennen.key; if (champ == summoner.data.Khazix.name) key = summoner.data.Khazix.key; if (champ == summoner.data.Kindred.name) key = summoner.data.Kindred.key; if (champ == summoner.data.Kled.name) key = summoner.data.Kled.key; if (champ == summoner.data.KogMaw.name) key = summoner.data.KogMaw.key; if (champ == summoner.data.Leblanc.name) key = summoner.data.Leblanc.key; if (champ == summoner.data.LeeSin.name) key = summoner.data.LeeSin.key; if (champ == summoner.data.Leona.name) key = summoner.data.Leona.key; if (champ == summoner.data.Lissandra.name) key = summoner.data.Lissandra.key; if (champ == summoner.data.Lucian.name) key = summoner.data.Lucian.key; if (champ == summoner.data.Lulu.name) key = summoner.data.Lulu.key; if (champ == summoner.data.Lux.name) key = summoner.data.Lux.key; if (champ == summoner.data.Malphite.name) key = summoner.data.Malphite.key; if (champ == summoner.data.Malzahar.name) key = summoner.data.Malzahar.key; if (champ == summoner.data.Maokai.name) key = summoner.data.Maokai.key; if (champ == summoner.data.MasterYi.name) key = summoner.data.MasterYi.key; if (champ == summoner.data.MissFortune.name) key = summoner.data.MissFortune.key; if (champ == summoner.data.Mordekaiser.name) key = summoner.data.Mordekaiser.key; if (champ == summoner.data.Morgana.name) key = summoner.data.Morgana.key; if (champ == summoner.data.Nami.name) key = summoner.data.Nami.key; if (champ == summoner.data.Nasus.name) key = summoner.data.Nasus.key; if (champ == summoner.data.Nautilus.name) key = summoner.data.Nautilus.key; if (champ == summoner.data.Neeko.name) key = summoner.data.Neeko.key; if (champ == summoner.data.Nidalee.name) key = summoner.data.Nidalee.key; if (champ == summoner.data.Nocturne.name) key = summoner.data.Nocturne.key; if (champ == summoner.data.Nunu.name) key = summoner.data.Nunu.key; if (champ == summoner.data.Olaf.name) key = summoner.data.Olaf.key; if (champ == summoner.data.Orianna.name) key = summoner.data.Orianna.key; if (champ == summoner.data.Ornn.name) key = summoner.data.Ornn.key; if (champ == summoner.data.Pantheon.name) key = summoner.data.Pantheon.key; if (champ == summoner.data.Poppy.name) key = summoner.data.Poppy.key; if (champ == summoner.data.Pyke.name) key = summoner.data.Pyke.key; if (champ == summoner.data.Quinn.name) key = summoner.data.Quinn.key; if (champ == summoner.data.Rakan.name) key = summoner.data.Rakan.key; if (champ == summoner.data.Rammus.name) key = summoner.data.Rammus.key; if (champ == summoner.data.RekSai.name) key = summoner.data.RekSai.key; if (champ == summoner.data.Renekton.name) key = summoner.data.Renekton.key; if (champ == summoner.data.Rengar.name) key = summoner.data.Rengar.key; if (champ == summoner.data.Riven.name) key = summoner.data.Riven.key; if (champ == summoner.data.Rumble.name) key = summoner.data.Rumble.key; if (champ == summoner.data.Ryze.name) key = summoner.data.Ryze.key; if (champ == summoner.data.Sejuani.name) key = summoner.data.Sejuani.key; if (champ == summoner.data.Shaco.name) key = summoner.data.Shaco.key; if (champ == summoner.data.Shen.name) key = summoner.data.Shen.key; if (champ == summoner.data.Shyvana.name) key = summoner.data.Shyvana.key; if (champ == summoner.data.Singed.name) key = summoner.data.Singed.key; if (champ == summoner.data.Sion.name) key = summoner.data.Sion.key; if (champ == summoner.data.Sivir.name) key = summoner.data.Sivir.key; if (champ == summoner.data.Skarner.name) key = summoner.data.Skarner.key; if (champ == summoner.data.Sona.name) key = summoner.data.Sona.key; if (champ == summoner.data.Soraka.name) key = summoner.data.Soraka.key; if (champ == summoner.data.Swain.name) key = summoner.data.Swain.key; if (champ == summoner.data.Sylas.name) key = summoner.data.Sylas.key; if (champ == summoner.data.Syndra.name) key = summoner.data.Syndra.key; if (champ == summoner.data.TahmKench.name) key = summoner.data.TahmKench.key; if (champ == summoner.data.Taliyah.name) key = summoner.data.Taliyah.key; if (champ == summoner.data.Talon.name) key = summoner.data.Talon.key; if (champ == summoner.data.Taric.name) key = summoner.data.Taric.key; if (champ == summoner.data.Teemo.name) key = summoner.data.Teemo.key; if (champ == summoner.data.Thresh.name) key = summoner.data.Thresh.key; if (champ == summoner.data.Tristana.name) key = summoner.data.Tristana.key; if (champ == summoner.data.Trundle.name) key = summoner.data.Trundle.key; if (champ == summoner.data.Tryndamere.name) key = summoner.data.Tryndamere.key; if (champ == summoner.data.TwistedFate.name) key = summoner.data.TwistedFate.key; if (champ == summoner.data.Twitch.name) key = summoner.data.Twitch.key; if (champ == summoner.data.Udyr.name) key = summoner.data.Udyr.key; if (champ == summoner.data.Urgot.name) key = summoner.data.Urgot.key; if (champ == summoner.data.Varus.name) key = summoner.data.Varus.key; if (champ == summoner.data.Vayne.name) key = summoner.data.Vayne.key; if (champ == summoner.data.Veigar.name) key = summoner.data.Veigar.key; if (champ == summoner.data.Velkoz.name) key = summoner.data.Velkoz.key; if (champ == summoner.data.Vi.name) key = summoner.data.Vi.key; if (champ == summoner.data.Viktor.name) key = summoner.data.Viktor.key; if (champ == summoner.data.Vladimir.name) key = summoner.data.Vladimir.key; if (champ == summoner.data.Volibear.name) key = summoner.data.Volibear.key; if (champ == summoner.data.Warwick.name) key = summoner.data.Warwick.key; if (champ == summoner.data.MonkeyKing.name) key = summoner.data.MonkeyKing.key; if (champ == summoner.data.Xayah.name) key = summoner.data.Xayah.key;
                label36.Text = key;
                switch (Num)
                {
                    case 1:
                        SummonerMatch2(accountId1, Num, key);
                        break;
                    case 2:
                        SummonerMatch2(accountId2, Num, key);
                        break;
                    case 3:
                        SummonerMatch2(accountId3, Num, key);
                        break;
                    case 4:
                        SummonerMatch2(accountId4, Num, key);
                        break;
                    case 5:
                        SummonerMatch2(accountId5, Num, key);
                        break;
                }

            }
        }
        public void SummonerMatch2(string accountId, int Num, string key)//소환사 기본 정보 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/match/v4/matchlists/by-account/" + accountId + "?api_key=" + api_key;
                string json_summonerInfo = client.DownloadString(url);
                SummonerMatch summoner = JsonConvert.DeserializeObject<SummonerMatch>(json_summonerInfo);
                List<Match> matches = summoner.matches;
                int m = 20;
                for (int i = 0; i < m; i++)
                {
                    if (matches[i].queue == 420)
                    {
                        if(Convert.ToString(matches[i].champion) == key)
                        {
                            string matchId = Convert.ToString(matches[i].gameId);
                            if (SummonerKda(matchId, accountId, Num) == 1)
                            {
                                m++;
                            }
                        }
                        
                    }
                    else
                    {
                        m++;
                    }
                }
                float rate = win / (win + loss) * 100;
                float kda = (kills + assists) / deaths;
                switch (Num-1)
                {
                    case 0:
                        label4.Text = rate.ToString("N0") + "%";
                        label3.Text = kda.ToString("N2") + ":1";
                        label2.Text = (cs / (win + loss)).ToString("N1") + " CS";
                        label1.Text = "총 " + Convert.ToString(win + loss) + " 판";
                        break;
                    case 1:
                        label15.Text = rate.ToString("N0") + "%";
                        label14.Text = kda.ToString("N2") + ":1";
                        label13.Text = (cs / 20).ToString("N1") + " CS";
                        label12.Text = "총 " + Convert.ToString(win + loss) + " 판";
                        break;
                    case 2:
                        label26.Text = rate.ToString("N0") + "%";
                        label25.Text = kda.ToString("N2") + ":1";
                        label24.Text = (cs / 20).ToString("N1") + " CS";
                        label23.Text = "총 " + Convert.ToString(win + loss) + " 판";
                        break;
                    case 3:
                        label32.Text = rate.ToString("N0") + "%";
                        label31.Text = kda.ToString("N2") + ":1";
                        label30.Text = (cs / 20).ToString("N1") + " CS";
                        label29.Text = "총 " + Convert.ToString(win + loss) + " 판";
                        break;
                    case 4:
                        label48.Text = rate.ToString("N0") + "%";
                        label47.Text = kda.ToString("N2") + ":1";
                        label46.Text = (cs / 20).ToString("N1") + " CS";
                        label45.Text = "총 " + Convert.ToString(win + loss) + " 판";
                        break;
                }
            }
        }

        public int SummonerKda2(string GameId, string accountid, int Num) //소환사 KDA, CS 표시 함수
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://kr.api.riotgames.com/lol/match/v4/matches/" + GameId + "?api_key=" + api_key;
                string json_summonerKda = client.DownloadString(url);
                SummonerKda summoner = JsonConvert.DeserializeObject<SummonerKda>(@json_summonerKda);
                List<ParticipantIdentity> participantIdentities = summoner.participantIdentities;
                List<Participant> participants = summoner.participants;
                if (summoner.gameDuration < 300)
                {
                    return 1;
                }
                int ans = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (participantIdentities[i].player.accountId == accountid)
                    {
                        ans = i;
                        break;
                    }
                }

                if (participants[ans].stats.win == true)
                {
                    win++;
                }
                else
                {
                    loss++;
                }

                kills += participants[ans].stats.kills;
                deaths += participants[ans].stats.deaths;
                assists += participants[ans].stats.assists;
                cs += participants[ans].stats.totalMinionsKilled + participants[ans].stats.neutralMinionsKilled;

            }
            return 0;
        }


        public Form1()
        {
            LOLState();
            InitializeComponent();
            comboBox1.Items.Add("가렌"); comboBox1.Items.Add("갈리오"); comboBox1.Items.Add("갱플랭크"); comboBox1.Items.Add("그라가스"); comboBox1.Items.Add("그레이브즈"); comboBox1.Items.Add("나르"); comboBox1.Items.Add("나미"); comboBox1.Items.Add("나서스"); comboBox1.Items.Add("노틸러스"); comboBox1.Items.Add("녹턴"); comboBox1.Items.Add("누누와 윌럼프"); comboBox1.Items.Add("니달리"); comboBox1.Items.Add("니코"); comboBox1.Items.Add("다리우스"); comboBox1.Items.Add("다이애나"); comboBox1.Items.Add("드레이븐"); comboBox1.Items.Add("라이즈"); comboBox1.Items.Add("라칸"); comboBox1.Items.Add("람머스"); comboBox1.Items.Add("럭스"); comboBox1.Items.Add("럼블"); comboBox1.Items.Add("레넥톤"); comboBox1.Items.Add("레오나"); comboBox1.Items.Add("렉사이"); comboBox1.Items.Add("렝가"); comboBox1.Items.Add("루시안"); comboBox1.Items.Add("룰루"); comboBox1.Items.Add("르블랑"); comboBox1.Items.Add("리 신"); comboBox1.Items.Add("리븐"); comboBox1.Items.Add("리산드라"); comboBox1.Items.Add("마스터 이"); comboBox1.Items.Add("마오카이"); comboBox1.Items.Add("말자하"); comboBox1.Items.Add("말파이트"); comboBox1.Items.Add("모데카이저"); comboBox1.Items.Add("모르가나"); comboBox1.Items.Add("문도 박사"); comboBox1.Items.Add("미스 포츈"); comboBox1.Items.Add("바드"); comboBox1.Items.Add("바루스"); comboBox1.Items.Add("바이"); comboBox1.Items.Add("베이가"); comboBox1.Items.Add("베인"); comboBox1.Items.Add("벨코즈"); comboBox1.Items.Add("볼리베어"); comboBox1.Items.Add("브라움"); comboBox1.Items.Add("브랜드"); comboBox1.Items.Add("블라디미르"); comboBox1.Items.Add("블리츠크랭크"); comboBox1.Items.Add("빅토르"); comboBox1.Items.Add("뽀삐"); comboBox1.Items.Add("사이온"); comboBox1.Items.Add("사일러스"); comboBox1.Items.Add("샤코"); comboBox1.Items.Add("세주아니"); comboBox1.Items.Add("소나"); comboBox1.Items.Add("소라카"); comboBox1.Items.Add("쉔"); comboBox1.Items.Add("쉬바나"); comboBox1.Items.Add("스웨인"); comboBox1.Items.Add("스카너"); comboBox1.Items.Add("시비르"); comboBox1.Items.Add("신"); comboBox1.Items.Add("짜오"); comboBox1.Items.Add("신드라"); comboBox1.Items.Add("신지드"); comboBox1.Items.Add("쓰레쉬"); comboBox1.Items.Add("아리"); comboBox1.Items.Add("아무무"); comboBox1.Items.Add("아우렐리온 솔"); comboBox1.Items.Add("아이번"); comboBox1.Items.Add("아지르"); comboBox1.Items.Add("아칼리"); comboBox1.Items.Add("아트록스"); comboBox1.Items.Add("알리스타"); comboBox1.Items.Add("애니"); comboBox1.Items.Add("애니비아"); comboBox1.Items.Add("애쉬"); comboBox1.Items.Add("야스오"); comboBox1.Items.Add("에코"); comboBox1.Items.Add("엘리스"); comboBox1.Items.Add("오공"); comboBox1.Items.Add("오른"); comboBox1.Items.Add("오리아나"); comboBox1.Items.Add("올라프"); comboBox1.Items.Add("요릭"); comboBox1.Items.Add("우디르"); comboBox1.Items.Add("우르곳"); comboBox1.Items.Add("워윅"); comboBox1.Items.Add("이렐리아"); comboBox1.Items.Add("이블린"); comboBox1.Items.Add("이즈리얼"); comboBox1.Items.Add("일라오이"); comboBox1.Items.Add("자르반 4세"); comboBox1.Items.Add("자야"); comboBox1.Items.Add("자이라"); comboBox1.Items.Add("자크"); comboBox1.Items.Add("잔나"); comboBox1.Items.Add("잭스"); comboBox1.Items.Add("제드"); comboBox1.Items.Add("제라스"); comboBox1.Items.Add("제이스"); comboBox1.Items.Add("조이"); comboBox1.Items.Add("직스"); comboBox1.Items.Add("진"); comboBox1.Items.Add("질리언"); comboBox1.Items.Add("징크스"); comboBox1.Items.Add("초가스"); comboBox1.Items.Add("카르마"); comboBox1.Items.Add("카밀"); comboBox1.Items.Add("카사딘"); comboBox1.Items.Add("카서스"); comboBox1.Items.Add("카시오페아"); comboBox1.Items.Add("카이사"); comboBox1.Items.Add("카직스"); comboBox1.Items.Add("카타리나"); comboBox1.Items.Add("칼리스타"); comboBox1.Items.Add("케넨"); comboBox1.Items.Add("케이틀린"); comboBox1.Items.Add("케인"); comboBox1.Items.Add("케일"); comboBox1.Items.Add("코그모"); comboBox1.Items.Add("코르키"); comboBox1.Items.Add("퀸"); comboBox1.Items.Add("클레드"); comboBox1.Items.Add("킨드레드"); comboBox1.Items.Add("타릭"); comboBox1.Items.Add("탈론"); comboBox1.Items.Add("탈리야"); comboBox1.Items.Add("탐 켄치"); comboBox1.Items.Add("트런들"); comboBox1.Items.Add("트리스타나"); comboBox1.Items.Add("트린다미어"); comboBox1.Items.Add("트위스티드 페이트"); comboBox1.Items.Add("트위치"); comboBox1.Items.Add("티모"); comboBox1.Items.Add("파이크"); comboBox1.Items.Add("판테온"); comboBox1.Items.Add("피들스틱"); comboBox1.Items.Add("피오라"); comboBox1.Items.Add("피즈"); comboBox1.Items.Add("하이머딩거"); comboBox1.Items.Add("헤카림");
            comboBox2.Items.Add("가렌"); comboBox2.Items.Add("갈리오"); comboBox2.Items.Add("갱플랭크"); comboBox2.Items.Add("그라가스"); comboBox2.Items.Add("그레이브즈"); comboBox2.Items.Add("나르"); comboBox2.Items.Add("나미"); comboBox2.Items.Add("나서스"); comboBox2.Items.Add("노틸러스"); comboBox2.Items.Add("녹턴"); comboBox2.Items.Add("누누와 윌럼프"); comboBox2.Items.Add("니달리"); comboBox2.Items.Add("니코"); comboBox2.Items.Add("다리우스"); comboBox2.Items.Add("다이애나"); comboBox2.Items.Add("드레이븐"); comboBox2.Items.Add("라이즈"); comboBox2.Items.Add("라칸"); comboBox2.Items.Add("람머스"); comboBox2.Items.Add("럭스"); comboBox2.Items.Add("럼블"); comboBox2.Items.Add("레넥톤"); comboBox2.Items.Add("레오나"); comboBox2.Items.Add("렉사이"); comboBox2.Items.Add("렝가"); comboBox2.Items.Add("루시안"); comboBox2.Items.Add("룰루"); comboBox2.Items.Add("르블랑"); comboBox2.Items.Add("리 신"); comboBox2.Items.Add("리븐"); comboBox2.Items.Add("리산드라"); comboBox2.Items.Add("마스터 이"); comboBox2.Items.Add("마오카이"); comboBox2.Items.Add("말자하"); comboBox2.Items.Add("말파이트"); comboBox2.Items.Add("모데카이저"); comboBox2.Items.Add("모르가나"); comboBox2.Items.Add("문도 박사"); comboBox2.Items.Add("미스 포츈"); comboBox2.Items.Add("바드"); comboBox2.Items.Add("바루스"); comboBox2.Items.Add("바이"); comboBox2.Items.Add("베이가"); comboBox2.Items.Add("베인"); comboBox2.Items.Add("벨코즈"); comboBox2.Items.Add("볼리베어"); comboBox2.Items.Add("브라움"); comboBox2.Items.Add("브랜드"); comboBox2.Items.Add("블라디미르"); comboBox2.Items.Add("블리츠크랭크"); comboBox2.Items.Add("빅토르"); comboBox2.Items.Add("뽀삐"); comboBox2.Items.Add("사이온"); comboBox2.Items.Add("사일러스"); comboBox2.Items.Add("샤코"); comboBox2.Items.Add("세주아니"); comboBox2.Items.Add("소나"); comboBox2.Items.Add("소라카"); comboBox2.Items.Add("쉔"); comboBox2.Items.Add("쉬바나"); comboBox2.Items.Add("스웨인"); comboBox2.Items.Add("스카너"); comboBox2.Items.Add("시비르"); comboBox2.Items.Add("신"); comboBox2.Items.Add("짜오"); comboBox2.Items.Add("신드라"); comboBox2.Items.Add("신지드"); comboBox2.Items.Add("쓰레쉬"); comboBox2.Items.Add("아리"); comboBox2.Items.Add("아무무"); comboBox2.Items.Add("아우렐리온 솔"); comboBox2.Items.Add("아이번"); comboBox2.Items.Add("아지르"); comboBox2.Items.Add("아칼리"); comboBox2.Items.Add("아트록스"); comboBox2.Items.Add("알리스타"); comboBox2.Items.Add("애니"); comboBox2.Items.Add("애니비아"); comboBox2.Items.Add("애쉬"); comboBox2.Items.Add("야스오"); comboBox2.Items.Add("에코"); comboBox2.Items.Add("엘리스"); comboBox2.Items.Add("오공"); comboBox2.Items.Add("오른"); comboBox2.Items.Add("오리아나"); comboBox2.Items.Add("올라프"); comboBox2.Items.Add("요릭"); comboBox2.Items.Add("우디르"); comboBox2.Items.Add("우르곳"); comboBox2.Items.Add("워윅"); comboBox2.Items.Add("이렐리아"); comboBox2.Items.Add("이블린"); comboBox2.Items.Add("이즈리얼"); comboBox2.Items.Add("일라오이"); comboBox2.Items.Add("자르반 4세"); comboBox2.Items.Add("자야"); comboBox2.Items.Add("자이라"); comboBox2.Items.Add("자크"); comboBox2.Items.Add("잔나"); comboBox2.Items.Add("잭스"); comboBox2.Items.Add("제드"); comboBox2.Items.Add("제라스"); comboBox2.Items.Add("제이스"); comboBox2.Items.Add("조이"); comboBox2.Items.Add("직스"); comboBox2.Items.Add("진"); comboBox2.Items.Add("질리언"); comboBox2.Items.Add("징크스"); comboBox2.Items.Add("초가스"); comboBox2.Items.Add("카르마"); comboBox2.Items.Add("카밀"); comboBox2.Items.Add("카사딘"); comboBox2.Items.Add("카서스"); comboBox2.Items.Add("카시오페아"); comboBox2.Items.Add("카이사"); comboBox2.Items.Add("카직스"); comboBox2.Items.Add("카타리나"); comboBox2.Items.Add("칼리스타"); comboBox2.Items.Add("케넨"); comboBox2.Items.Add("케이틀린"); comboBox2.Items.Add("케인"); comboBox2.Items.Add("케일"); comboBox2.Items.Add("코그모"); comboBox2.Items.Add("코르키"); comboBox2.Items.Add("퀸"); comboBox2.Items.Add("클레드"); comboBox2.Items.Add("킨드레드"); comboBox2.Items.Add("타릭"); comboBox2.Items.Add("탈론"); comboBox2.Items.Add("탈리야"); comboBox2.Items.Add("탐 켄치"); comboBox2.Items.Add("트런들"); comboBox2.Items.Add("트리스타나"); comboBox2.Items.Add("트린다미어"); comboBox2.Items.Add("트위스티드 페이트"); comboBox2.Items.Add("트위치"); comboBox2.Items.Add("티모"); comboBox2.Items.Add("파이크"); comboBox2.Items.Add("판테온"); comboBox2.Items.Add("피들스틱"); comboBox2.Items.Add("피오라"); comboBox2.Items.Add("피즈"); comboBox2.Items.Add("하이머딩거"); comboBox2.Items.Add("헤카림");
            comboBox3.Items.Add("가렌"); comboBox3.Items.Add("갈리오"); comboBox3.Items.Add("갱플랭크"); comboBox3.Items.Add("그라가스"); comboBox3.Items.Add("그레이브즈"); comboBox3.Items.Add("나르"); comboBox3.Items.Add("나미"); comboBox3.Items.Add("나서스"); comboBox3.Items.Add("노틸러스"); comboBox3.Items.Add("녹턴"); comboBox3.Items.Add("누누와 윌럼프"); comboBox3.Items.Add("니달리"); comboBox3.Items.Add("니코"); comboBox3.Items.Add("다리우스"); comboBox3.Items.Add("다이애나"); comboBox3.Items.Add("드레이븐"); comboBox3.Items.Add("라이즈"); comboBox3.Items.Add("라칸"); comboBox3.Items.Add("람머스"); comboBox3.Items.Add("럭스"); comboBox3.Items.Add("럼블"); comboBox3.Items.Add("레넥톤"); comboBox3.Items.Add("레오나"); comboBox3.Items.Add("렉사이"); comboBox3.Items.Add("렝가"); comboBox3.Items.Add("루시안"); comboBox3.Items.Add("룰루"); comboBox3.Items.Add("르블랑"); comboBox3.Items.Add("리 신"); comboBox3.Items.Add("리븐"); comboBox3.Items.Add("리산드라"); comboBox3.Items.Add("마스터 이"); comboBox3.Items.Add("마오카이"); comboBox3.Items.Add("말자하"); comboBox3.Items.Add("말파이트"); comboBox3.Items.Add("모데카이저"); comboBox3.Items.Add("모르가나"); comboBox3.Items.Add("문도 박사"); comboBox3.Items.Add("미스 포츈"); comboBox3.Items.Add("바드"); comboBox3.Items.Add("바루스"); comboBox3.Items.Add("바이"); comboBox3.Items.Add("베이가"); comboBox3.Items.Add("베인"); comboBox3.Items.Add("벨코즈"); comboBox3.Items.Add("볼리베어"); comboBox3.Items.Add("브라움"); comboBox3.Items.Add("브랜드"); comboBox3.Items.Add("블라디미르"); comboBox3.Items.Add("블리츠크랭크"); comboBox3.Items.Add("빅토르"); comboBox3.Items.Add("뽀삐"); comboBox3.Items.Add("사이온"); comboBox3.Items.Add("사일러스"); comboBox3.Items.Add("샤코"); comboBox3.Items.Add("세주아니"); comboBox3.Items.Add("소나"); comboBox3.Items.Add("소라카"); comboBox3.Items.Add("쉔"); comboBox3.Items.Add("쉬바나"); comboBox3.Items.Add("스웨인"); comboBox3.Items.Add("스카너"); comboBox3.Items.Add("시비르"); comboBox3.Items.Add("신"); comboBox3.Items.Add("짜오"); comboBox3.Items.Add("신드라"); comboBox3.Items.Add("신지드"); comboBox3.Items.Add("쓰레쉬"); comboBox3.Items.Add("아리"); comboBox3.Items.Add("아무무"); comboBox3.Items.Add("아우렐리온 솔"); comboBox3.Items.Add("아이번"); comboBox3.Items.Add("아지르"); comboBox3.Items.Add("아칼리"); comboBox3.Items.Add("아트록스"); comboBox3.Items.Add("알리스타"); comboBox3.Items.Add("애니"); comboBox3.Items.Add("애니비아"); comboBox3.Items.Add("애쉬"); comboBox3.Items.Add("야스오"); comboBox3.Items.Add("에코"); comboBox3.Items.Add("엘리스"); comboBox3.Items.Add("오공"); comboBox3.Items.Add("오른"); comboBox3.Items.Add("오리아나"); comboBox3.Items.Add("올라프"); comboBox3.Items.Add("요릭"); comboBox3.Items.Add("우디르"); comboBox3.Items.Add("우르곳"); comboBox3.Items.Add("워윅"); comboBox3.Items.Add("이렐리아"); comboBox3.Items.Add("이블린"); comboBox3.Items.Add("이즈리얼"); comboBox3.Items.Add("일라오이"); comboBox3.Items.Add("자르반 4세"); comboBox3.Items.Add("자야"); comboBox3.Items.Add("자이라"); comboBox3.Items.Add("자크"); comboBox3.Items.Add("잔나"); comboBox3.Items.Add("잭스"); comboBox3.Items.Add("제드"); comboBox3.Items.Add("제라스"); comboBox3.Items.Add("제이스"); comboBox3.Items.Add("조이"); comboBox3.Items.Add("직스"); comboBox3.Items.Add("진"); comboBox3.Items.Add("질리언"); comboBox3.Items.Add("징크스"); comboBox3.Items.Add("초가스"); comboBox3.Items.Add("카르마"); comboBox3.Items.Add("카밀"); comboBox3.Items.Add("카사딘"); comboBox3.Items.Add("카서스"); comboBox3.Items.Add("카시오페아"); comboBox3.Items.Add("카이사"); comboBox3.Items.Add("카직스"); comboBox3.Items.Add("카타리나"); comboBox3.Items.Add("칼리스타"); comboBox3.Items.Add("케넨"); comboBox3.Items.Add("케이틀린"); comboBox3.Items.Add("케인"); comboBox3.Items.Add("케일"); comboBox3.Items.Add("코그모"); comboBox3.Items.Add("코르키"); comboBox3.Items.Add("퀸"); comboBox3.Items.Add("클레드"); comboBox3.Items.Add("킨드레드"); comboBox3.Items.Add("타릭"); comboBox3.Items.Add("탈론"); comboBox3.Items.Add("탈리야"); comboBox3.Items.Add("탐 켄치"); comboBox3.Items.Add("트런들"); comboBox3.Items.Add("트리스타나"); comboBox3.Items.Add("트린다미어"); comboBox3.Items.Add("트위스티드 페이트"); comboBox3.Items.Add("트위치"); comboBox3.Items.Add("티모"); comboBox3.Items.Add("파이크"); comboBox3.Items.Add("판테온"); comboBox3.Items.Add("피들스틱"); comboBox3.Items.Add("피오라"); comboBox3.Items.Add("피즈"); comboBox3.Items.Add("하이머딩거"); comboBox3.Items.Add("헤카림");
            comboBox4.Items.Add("가렌"); comboBox4.Items.Add("갈리오"); comboBox4.Items.Add("갱플랭크"); comboBox4.Items.Add("그라가스"); comboBox4.Items.Add("그레이브즈"); comboBox4.Items.Add("나르"); comboBox4.Items.Add("나미"); comboBox4.Items.Add("나서스"); comboBox4.Items.Add("노틸러스"); comboBox4.Items.Add("녹턴"); comboBox4.Items.Add("누누와 윌럼프"); comboBox4.Items.Add("니달리"); comboBox4.Items.Add("니코"); comboBox4.Items.Add("다리우스"); comboBox4.Items.Add("다이애나"); comboBox4.Items.Add("드레이븐"); comboBox4.Items.Add("라이즈"); comboBox4.Items.Add("라칸"); comboBox4.Items.Add("람머스"); comboBox4.Items.Add("럭스"); comboBox4.Items.Add("럼블"); comboBox4.Items.Add("레넥톤"); comboBox4.Items.Add("레오나"); comboBox4.Items.Add("렉사이"); comboBox4.Items.Add("렝가"); comboBox4.Items.Add("루시안"); comboBox4.Items.Add("룰루"); comboBox4.Items.Add("르블랑"); comboBox4.Items.Add("리 신"); comboBox4.Items.Add("리븐"); comboBox4.Items.Add("리산드라"); comboBox4.Items.Add("마스터 이"); comboBox4.Items.Add("마오카이"); comboBox4.Items.Add("말자하"); comboBox4.Items.Add("말파이트"); comboBox4.Items.Add("모데카이저"); comboBox4.Items.Add("모르가나"); comboBox4.Items.Add("문도 박사"); comboBox4.Items.Add("미스 포츈"); comboBox4.Items.Add("바드"); comboBox4.Items.Add("바루스"); comboBox4.Items.Add("바이"); comboBox4.Items.Add("베이가"); comboBox4.Items.Add("베인"); comboBox4.Items.Add("벨코즈"); comboBox4.Items.Add("볼리베어"); comboBox4.Items.Add("브라움"); comboBox4.Items.Add("브랜드"); comboBox4.Items.Add("블라디미르"); comboBox4.Items.Add("블리츠크랭크"); comboBox4.Items.Add("빅토르"); comboBox4.Items.Add("뽀삐"); comboBox4.Items.Add("사이온"); comboBox4.Items.Add("사일러스"); comboBox4.Items.Add("샤코"); comboBox4.Items.Add("세주아니"); comboBox4.Items.Add("소나"); comboBox4.Items.Add("소라카"); comboBox4.Items.Add("쉔"); comboBox4.Items.Add("쉬바나"); comboBox4.Items.Add("스웨인"); comboBox4.Items.Add("스카너"); comboBox4.Items.Add("시비르"); comboBox4.Items.Add("신"); comboBox4.Items.Add("짜오"); comboBox4.Items.Add("신드라"); comboBox4.Items.Add("신지드"); comboBox4.Items.Add("쓰레쉬"); comboBox4.Items.Add("아리"); comboBox4.Items.Add("아무무"); comboBox4.Items.Add("아우렐리온 솔"); comboBox4.Items.Add("아이번"); comboBox4.Items.Add("아지르"); comboBox4.Items.Add("아칼리"); comboBox4.Items.Add("아트록스"); comboBox4.Items.Add("알리스타"); comboBox4.Items.Add("애니"); comboBox4.Items.Add("애니비아"); comboBox4.Items.Add("애쉬"); comboBox4.Items.Add("야스오"); comboBox4.Items.Add("에코"); comboBox4.Items.Add("엘리스"); comboBox4.Items.Add("오공"); comboBox4.Items.Add("오른"); comboBox4.Items.Add("오리아나"); comboBox4.Items.Add("올라프"); comboBox4.Items.Add("요릭"); comboBox4.Items.Add("우디르"); comboBox4.Items.Add("우르곳"); comboBox4.Items.Add("워윅"); comboBox4.Items.Add("이렐리아"); comboBox4.Items.Add("이블린"); comboBox4.Items.Add("이즈리얼"); comboBox4.Items.Add("일라오이"); comboBox4.Items.Add("자르반 4세"); comboBox4.Items.Add("자야"); comboBox4.Items.Add("자이라"); comboBox4.Items.Add("자크"); comboBox4.Items.Add("잔나"); comboBox4.Items.Add("잭스"); comboBox4.Items.Add("제드"); comboBox4.Items.Add("제라스"); comboBox4.Items.Add("제이스"); comboBox4.Items.Add("조이"); comboBox4.Items.Add("직스"); comboBox4.Items.Add("진"); comboBox4.Items.Add("질리언"); comboBox4.Items.Add("징크스"); comboBox4.Items.Add("초가스"); comboBox4.Items.Add("카르마"); comboBox4.Items.Add("카밀"); comboBox4.Items.Add("카사딘"); comboBox4.Items.Add("카서스"); comboBox4.Items.Add("카시오페아"); comboBox4.Items.Add("카이사"); comboBox4.Items.Add("카직스"); comboBox4.Items.Add("카타리나"); comboBox4.Items.Add("칼리스타"); comboBox4.Items.Add("케넨"); comboBox4.Items.Add("케이틀린"); comboBox4.Items.Add("케인"); comboBox4.Items.Add("케일"); comboBox4.Items.Add("코그모"); comboBox4.Items.Add("코르키"); comboBox4.Items.Add("퀸"); comboBox4.Items.Add("클레드"); comboBox4.Items.Add("킨드레드"); comboBox4.Items.Add("타릭"); comboBox4.Items.Add("탈론"); comboBox4.Items.Add("탈리야"); comboBox4.Items.Add("탐 켄치"); comboBox4.Items.Add("트런들"); comboBox4.Items.Add("트리스타나"); comboBox4.Items.Add("트린다미어"); comboBox4.Items.Add("트위스티드 페이트"); comboBox4.Items.Add("트위치"); comboBox4.Items.Add("티모"); comboBox4.Items.Add("파이크"); comboBox4.Items.Add("판테온"); comboBox4.Items.Add("피들스틱"); comboBox4.Items.Add("피오라"); comboBox4.Items.Add("피즈"); comboBox4.Items.Add("하이머딩거"); comboBox4.Items.Add("헤카림");
            comboBox5.Items.Add("가렌"); comboBox5.Items.Add("갈리오"); comboBox5.Items.Add("갱플랭크"); comboBox5.Items.Add("그라가스"); comboBox5.Items.Add("그레이브즈"); comboBox5.Items.Add("나르"); comboBox5.Items.Add("나미"); comboBox5.Items.Add("나서스"); comboBox5.Items.Add("노틸러스"); comboBox5.Items.Add("녹턴"); comboBox5.Items.Add("누누와 윌럼프"); comboBox5.Items.Add("니달리"); comboBox5.Items.Add("니코"); comboBox5.Items.Add("다리우스"); comboBox5.Items.Add("다이애나"); comboBox5.Items.Add("드레이븐"); comboBox5.Items.Add("라이즈"); comboBox5.Items.Add("라칸"); comboBox5.Items.Add("람머스"); comboBox5.Items.Add("럭스"); comboBox5.Items.Add("럼블"); comboBox5.Items.Add("레넥톤"); comboBox5.Items.Add("레오나"); comboBox5.Items.Add("렉사이"); comboBox5.Items.Add("렝가"); comboBox5.Items.Add("루시안"); comboBox5.Items.Add("룰루"); comboBox5.Items.Add("르블랑"); comboBox5.Items.Add("리 신"); comboBox5.Items.Add("리븐"); comboBox5.Items.Add("리산드라"); comboBox5.Items.Add("마스터 이"); comboBox5.Items.Add("마오카이"); comboBox5.Items.Add("말자하"); comboBox5.Items.Add("말파이트"); comboBox5.Items.Add("모데카이저"); comboBox5.Items.Add("모르가나"); comboBox5.Items.Add("문도 박사"); comboBox5.Items.Add("미스 포츈"); comboBox5.Items.Add("바드"); comboBox5.Items.Add("바루스"); comboBox5.Items.Add("바이"); comboBox5.Items.Add("베이가"); comboBox5.Items.Add("베인"); comboBox5.Items.Add("벨코즈"); comboBox5.Items.Add("볼리베어"); comboBox5.Items.Add("브라움"); comboBox5.Items.Add("브랜드"); comboBox5.Items.Add("블라디미르"); comboBox5.Items.Add("블리츠크랭크"); comboBox5.Items.Add("빅토르"); comboBox5.Items.Add("뽀삐"); comboBox5.Items.Add("사이온"); comboBox5.Items.Add("사일러스"); comboBox5.Items.Add("샤코"); comboBox5.Items.Add("세주아니"); comboBox5.Items.Add("소나"); comboBox5.Items.Add("소라카"); comboBox5.Items.Add("쉔"); comboBox5.Items.Add("쉬바나"); comboBox5.Items.Add("스웨인"); comboBox5.Items.Add("스카너"); comboBox5.Items.Add("시비르"); comboBox5.Items.Add("신"); comboBox5.Items.Add("짜오"); comboBox5.Items.Add("신드라"); comboBox5.Items.Add("신지드"); comboBox5.Items.Add("쓰레쉬"); comboBox5.Items.Add("아리"); comboBox5.Items.Add("아무무"); comboBox5.Items.Add("아우렐리온 솔"); comboBox5.Items.Add("아이번"); comboBox5.Items.Add("아지르"); comboBox5.Items.Add("아칼리"); comboBox5.Items.Add("아트록스"); comboBox5.Items.Add("알리스타"); comboBox5.Items.Add("애니"); comboBox5.Items.Add("애니비아"); comboBox5.Items.Add("애쉬"); comboBox5.Items.Add("야스오"); comboBox5.Items.Add("에코"); comboBox5.Items.Add("엘리스"); comboBox5.Items.Add("오공"); comboBox5.Items.Add("오른"); comboBox5.Items.Add("오리아나"); comboBox5.Items.Add("올라프"); comboBox5.Items.Add("요릭"); comboBox5.Items.Add("우디르"); comboBox5.Items.Add("우르곳"); comboBox5.Items.Add("워윅"); comboBox5.Items.Add("이렐리아"); comboBox5.Items.Add("이블린"); comboBox5.Items.Add("이즈리얼"); comboBox5.Items.Add("일라오이"); comboBox5.Items.Add("자르반 4세"); comboBox5.Items.Add("자야"); comboBox5.Items.Add("자이라"); comboBox5.Items.Add("자크"); comboBox5.Items.Add("잔나"); comboBox5.Items.Add("잭스"); comboBox5.Items.Add("제드"); comboBox5.Items.Add("제라스"); comboBox5.Items.Add("제이스"); comboBox5.Items.Add("조이"); comboBox5.Items.Add("직스"); comboBox5.Items.Add("진"); comboBox5.Items.Add("질리언"); comboBox5.Items.Add("징크스"); comboBox5.Items.Add("초가스"); comboBox5.Items.Add("카르마"); comboBox5.Items.Add("카밀"); comboBox5.Items.Add("카사딘"); comboBox5.Items.Add("카서스"); comboBox5.Items.Add("카시오페아"); comboBox5.Items.Add("카이사"); comboBox5.Items.Add("카직스"); comboBox5.Items.Add("카타리나"); comboBox5.Items.Add("칼리스타"); comboBox5.Items.Add("케넨"); comboBox5.Items.Add("케이틀린"); comboBox5.Items.Add("케인"); comboBox5.Items.Add("케일"); comboBox5.Items.Add("코그모"); comboBox5.Items.Add("코르키"); comboBox5.Items.Add("퀸"); comboBox5.Items.Add("클레드"); comboBox5.Items.Add("킨드레드"); comboBox5.Items.Add("타릭"); comboBox5.Items.Add("탈론"); comboBox5.Items.Add("탈리야"); comboBox5.Items.Add("탐 켄치"); comboBox5.Items.Add("트런들"); comboBox5.Items.Add("트리스타나"); comboBox5.Items.Add("트린다미어"); comboBox5.Items.Add("트위스티드 페이트"); comboBox5.Items.Add("트위치"); comboBox5.Items.Add("티모"); comboBox5.Items.Add("파이크"); comboBox5.Items.Add("판테온"); comboBox5.Items.Add("피들스틱"); comboBox5.Items.Add("피오라"); comboBox5.Items.Add("피즈"); comboBox5.Items.Add("하이머딩거"); comboBox5.Items.Add("헤카림");
            
        }


        private void button1_Click(object sender, EventArgs e)//검색 버튼
        {
            string Name = textBox1.Text;
            string[] Names = Name.Split('\r');
            progressBar1.Value = 0;

            for (int i = 0; i < Names.Length; i++)
            {
                progressBar1.Value += 10;
                if (Names[i].Contains("님이 로비에 참가하셨습니다.") == false)//올바른 입력인지 검사
                {
                    continue;
                }
                else
                {
                    Names[i] = Names[i].Replace("님이 로비에 참가하셨습니다.", "");
                    Names[i] = Names[i].Replace("\n", "");
                    kills = 0;
                    deaths = 0;
                    assists = 0;
                    cs = 0;
                    win = 0;
                    loss = 0;
                    winning = 0;
                    winning_check = true;
                    SummonerInfo(Names[i], i);//소환사 기본 정보 표시 함수로 이동
                }

            }
            progressBar1.Value = 100;
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            kills = 0;
            deaths = 0;
            assists = 0;
            cs = 0;
            win = 0;
            loss = 0;
            LOLChamp(Convert.ToString(comboBox1.SelectedItem), 1);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            kills = 0;
            deaths = 0;
            assists = 0;
            cs = 0;
            win = 0;
            loss = 0;
            LOLChamp(Convert.ToString(comboBox2.SelectedItem), 2);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            kills = 0;
            deaths = 0;
            assists = 0;
            cs = 0;
            win = 0;
            loss = 0;
            LOLChamp(Convert.ToString(comboBox3.SelectedItem), 3);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            kills = 0;
            deaths = 0;
            assists = 0;
            cs = 0;
            win = 0;
            loss = 0;
            LOLChamp(Convert.ToString(comboBox4.SelectedItem), 4);
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            kills = 0;
            deaths = 0;
            assists = 0;
            cs = 0;
            win = 0;
            loss = 0;
            LOLChamp(Convert.ToString(comboBox5.SelectedItem), 5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.Show();
        }
    }
}