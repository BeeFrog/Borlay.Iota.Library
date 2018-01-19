﻿using Borlay.Iota.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Borlay.Iota.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Borlay.Iota.Library.Crypto;

namespace Borlay.Iota.Library.Tests
{
    [TestClass]
    public class TransactionItemTests
    {
        [TestMethod]
        public void Transaction_FromTrytes_DeserialisesCorrectly()
        {
            // Arrange
            var trytes = "MTWLHBKJJLIRJVEWDPOWHPACMACNTIBTNPLKKTRJHKAA9UAKFH9XCZTDPHIPCXNZ9KQIZCMJXEFUACJXWOZVZHOFYWJMSOSHJYHEBXUIG9TBDMVKSYAUDQAXSDDFPKNHOSETSRNCSSMMMLHGYDQLQWRQWBBOJSWMVXXYIOMI9EXTPJYIQ9GXCLZMKG9UNFKLKRGGJMIMMVGZGBDGRKWBHXCZOMCIRVLBMGXQKXEQKZNMVYHODWXJYYPVUQLLBGCILOYKHBFRUJKIKMKH9RITQOQGLESBZGDDQLCYOUTCRFHJ9ARQYPQLRLOHDEAXEYTM9CCCRWGCJWRGAWYHSHBFXOPNKCVSHDCWPMTSZOUMNOTDWFEDLMSLLMWQBAVAPMPBGZ9HSJOPPFCRPDDUVBCYWKRAWFYVGWJEIQOTXUXSWCWKRQHBWSNZVIOIFFTSQNSOZYEQZJ9NQFERBTITOCPRUQAWAKWIM9MIRKNWVBGXQOIJANCKPOFGEBYCLYNPYMKVBZAYOMKUZJGEQFQIIZLJPZIZMQFDXOUJTAZUEDWZICGDLKCUVNEERXDPAMDEPLUC9NKOAJLQLVIIAVHYHGGJQEPVLKWSXPLZCRDILBKRCFLNLQWLOYEBABWRNQELYMKETIHEYAVDRBSFFVFGKAVZEWITNMBIHIVTHWLRKPTCRWG9FIU9WZJAVXMNMXQM9KACRFIKTSCVPDAZITVSFOOLFIRMBJKHHV9JLQBVRVBPYGCGJZBGVNKKGBMEKMFSNPQWVKAU99ZEXYRBFFOWYDJBAKE9HFYJCSLRXGZDWLMXA9AHZFLOZXTZKMKYAYZGKVMWUJANGYRQAFVTHMLCQNNRFLJSRHHYHIICFJZTNRMLYBKDOWMRWCPTUX9YGLWPSFQUBBTOEXLQGD9AHFL9QH9JCU99AQXCROUOTACDUMRPXVCGGLASE9LQSKP9SQBJGNNRRJFPESFWESFWJJRDVIVFTT9GKGANNXGVR9FXAENOIKLQ9KBEHSIWXTDMZEVBVHXXFBZ9XOFAIVVHIYNHDPPCBIWBNAAPWNYNAETHXWCZUFARSMSKANTRVBTAJUWLUBNXRBEMHSMYYCFJSOHIUJJGLBGTHJCPWJHUWWKJMPJWVUGCAXNVO9IHEUBXTQDOUHGLCRGNWIIHUTUQIPSDUSONGMGIU9IZUMRUEIIYZKFFQWLNYQCHPKBMQHECTHLSCUDBXO9BGMIKILFTDZMJFHDFWSZSFVQIOPBQUXQRXSMVYNJTGYV9LDGMNKELFOMAIGRXXNQYURLYSKYKCIFZPLYIBMCUARIRK9RAUUJAU9GJKLYMYOCYKCIWDAVFEKSRHSRSTW9IMHQEXZGWTZJYVRBMGLNZHSBBPQF9BPJSNJHYQC9XUSPGUVQZAURAPDEIIVEVNOW9NHQRWHNEWNZDFWRLRMKSKGSOQUSKROMGTOTSIMJUSSVNFBMAUFHOKOTYSEIMPFRCGJDI9NHLUUJHYMFY9RESEXFRMEPMMINDOKIBCNKGLBOLABOQXFWCUJYWQDRD9AADGBSCRCXERVQC9C9JOMVYQROULJTFTQVMVOSZ9YQIRPRQSTHSMNCNNZWHFWCLHVXADOU9N9QSFUIHIZXWJWWUMGQYZJTBKLXMZBCXPWBUZWDYJHUIRSVZTOAWV9EYTPCY9TTEKMIJQZSUVVCEDIHCRPTNWVEYNUUN9WXWQYIZUZJFETKSOKOT9CBFNKKRNJXZHHJGOWXHBULQMXMCNSLDHBGBMKHHMNOGFOTXZICOGGRJH9KZ9YDYZJCERMKLOCROVNGSZRKPHZJNAZQSTCUIYCYL9NS9RMKFIGKH9G9SRRZKLVDLNKU9PTEZGUEGGTKRJODNQYGEALTQOOBSOVCEBAZZWITGKMJUQODVKFDNTQSDKDYVBGUJNUIEDMSSOYLWVPAYGJH9ANL9VHGOAHXDKYPOWP9DGMUOBFZKHIMAHOBXMHMGNLVXZQDGYCVLZOEHOYNKKNLKPMAEPTUOBGRXXKBTCPKGNMSHXUDSWQOPRTLVJAFSEXDOMHENM9EMQJHL9DG9LRRMKGHMLTYX9S9EFOYHQLLGMJJEWF9KRYVNAGXFRZCRMCOVMXDYWWPUIKDYPASAECDPFSHFDTGTDNPXD9WZLNOIDLSD9TY9KUDUGVDTTAK9JQTJZU9KFLYD9VXGLBBGMEXASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXWMJLNY9999999999999999999999MINEIOTADOTCOM9999999999999X9DVQYD99C99999999E999999999BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZGFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999MINEIOTADOTCOM9999999999999DBNIZZXJE999999999L99999999XPBFUCTLEMOYKHMTZOKTNLTTHJY";

            // Act
            var tran = new TransactionItem(trytes);

            // Assert
            Assert.AreEqual("ASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXW", tran.Address);
            Assert.AreEqual("-1309730", tran.Value);
            Assert.AreEqual("MINEIOTADOTCOM9999999999999", tran.Tag);
            Assert.AreEqual(1515574230, tran.Timestamp);

            Assert.AreEqual("9BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZ", tran.Bundle);
            Assert.AreEqual("GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999", tran.TrunkTransaction);
            Assert.AreEqual("NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999", tran.BranchTransaction);
            Assert.AreEqual("XPBFUCTLEMOYKHMTZOKTNLTTHJY", tran.Nonce);
            Assert.AreEqual("MTWLHBKJJLIRJVEWDPOWHPACMACNTIBTNPLKKTRJHKAA9UAKFH9XCZTDPHIPCXNZ9KQIZCMJXEFUACJXWOZVZHOFYWJMSOSHJYHEBXUIG9TBDMVKSYAUDQAXSDDFPKNHOSETSRNCSSMMMLHGYDQLQWRQWBBOJSWMVXXYIOMI9EXTPJYIQ9GXCLZMKG9UNFKLKRGGJMIMMVGZGBDGRKWBHXCZOMCIRVLBMGXQKXEQKZNMVYHODWXJYYPVUQLLBGCILOYKHBFRUJKIKMKH9RITQOQGLESBZGDDQLCYOUTCRFHJ9ARQYPQLRLOHDEAXEYTM9CCCRWGCJWRGAWYHSHBFXOPNKCVSHDCWPMTSZOUMNOTDWFEDLMSLLMWQBAVAPMPBGZ9HSJOPPFCRPDDUVBCYWKRAWFYVGWJEIQOTXUXSWCWKRQHBWSNZVIOIFFTSQNSOZYEQZJ9NQFERBTITOCPRUQAWAKWIM9MIRKNWVBGXQOIJANCKPOFGEBYCLYNPYMKVBZAYOMKUZJGEQFQIIZLJPZIZMQFDXOUJTAZUEDWZICGDLKCUVNEERXDPAMDEPLUC9NKOAJLQLVIIAVHYHGGJQEPVLKWSXPLZCRDILBKRCFLNLQWLOYEBABWRNQELYMKETIHEYAVDRBSFFVFGKAVZEWITNMBIHIVTHWLRKPTCRWG9FIU9WZJAVXMNMXQM9KACRFIKTSCVPDAZITVSFOOLFIRMBJKHHV9JLQBVRVBPYGCGJZBGVNKKGBMEKMFSNPQWVKAU99ZEXYRBFFOWYDJBAKE9HFYJCSLRXGZDWLMXA9AHZFLOZXTZKMKYAYZGKVMWUJANGYRQAFVTHMLCQNNRFLJSRHHYHIICFJZTNRMLYBKDOWMRWCPTUX9YGLWPSFQUBBTOEXLQGD9AHFL9QH9JCU99AQXCROUOTACDUMRPXVCGGLASE9LQSKP9SQBJGNNRRJFPESFWESFWJJRDVIVFTT9GKGANNXGVR9FXAENOIKLQ9KBEHSIWXTDMZEVBVHXXFBZ9XOFAIVVHIYNHDPPCBIWBNAAPWNYNAETHXWCZUFARSMSKANTRVBTAJUWLUBNXRBEMHSMYYCFJSOHIUJJGLBGTHJCPWJHUWWKJMPJWVUGCAXNVO9IHEUBXTQDOUHGLCRGNWIIHUTUQIPSDUSONGMGIU9IZUMRUEIIYZKFFQWLNYQCHPKBMQHECTHLSCUDBXO9BGMIKILFTDZMJFHDFWSZSFVQIOPBQUXQRXSMVYNJTGYV9LDGMNKELFOMAIGRXXNQYURLYSKYKCIFZPLYIBMCUARIRK9RAUUJAU9GJKLYMYOCYKCIWDAVFEKSRHSRSTW9IMHQEXZGWTZJYVRBMGLNZHSBBPQF9BPJSNJHYQC9XUSPGUVQZAURAPDEIIVEVNOW9NHQRWHNEWNZDFWRLRMKSKGSOQUSKROMGTOTSIMJUSSVNFBMAUFHOKOTYSEIMPFRCGJDI9NHLUUJHYMFY9RESEXFRMEPMMINDOKIBCNKGLBOLABOQXFWCUJYWQDRD9AADGBSCRCXERVQC9C9JOMVYQROULJTFTQVMVOSZ9YQIRPRQSTHSMNCNNZWHFWCLHVXADOU9N9QSFUIHIZXWJWWUMGQYZJTBKLXMZBCXPWBUZWDYJHUIRSVZTOAWV9EYTPCY9TTEKMIJQZSUVVCEDIHCRPTNWVEYNUUN9WXWQYIZUZJFETKSOKOT9CBFNKKRNJXZHHJGOWXHBULQMXMCNSLDHBGBMKHHMNOGFOTXZICOGGRJH9KZ9YDYZJCERMKLOCROVNGSZRKPHZJNAZQSTCUIYCYL9NS9RMKFIGKH9G9SRRZKLVDLNKU9PTEZGUEGGTKRJODNQYGEALTQOOBSOVCEBAZZWITGKMJUQODVKFDNTQSDKDYVBGUJNUIEDMSSOYLWVPAYGJH9ANL9VHGOAHXDKYPOWP9DGMUOBFZKHIMAHOBXMHMGNLVXZQDGYCVLZOEHOYNKKNLKPMAEPTUOBGRXXKBTCPKGNMSHXUDSWQOPRTLVJAFSEXDOMHENM9EMQJHL9DG9LRRMKGHMLTYX9S9EFOYHQLLGMJJEWF9KRYVNAGXFRZCRMCOVMXDYWWPUIKDYPASAECDPFSHFDTGTDNPXD9WZLNOIDLSD9TY9KUDUGVDTTAK9JQTJZU9KFLYD9VXGLBBGMEX", tran.SignatureFragment);

            Assert.AreEqual("5", tran.LastIndex);
            Assert.AreEqual("3", tran.CurrentIndex);
            Assert.AreEqual("EQK9ZETZMFMB9VMIEYEXKTAEMZBGQSDSAMCWXKZVVQXKZBUPGCJ9RRQLMWTEKLBWKNERC9ZMIROXZ9999", tran.Hash);
        }

        [TestMethod]
        public void Transaction_ToTrytes_SerialisesCorrectly()
        {
            // Arrange
            var expectedtrytes = "MTWLHBKJJLIRJVEWDPOWHPACMACNTIBTNPLKKTRJHKAA9UAKFH9XCZTDPHIPCXNZ9KQIZCMJXEFUACJXWOZVZHOFYWJMSOSHJYHEBXUIG9TBDMVKSYAUDQAXSDDFPKNHOSETSRNCSSMMMLHGYDQLQWRQWBBOJSWMVXXYIOMI9EXTPJYIQ9GXCLZMKG9UNFKLKRGGJMIMMVGZGBDGRKWBHXCZOMCIRVLBMGXQKXEQKZNMVYHODWXJYYPVUQLLBGCILOYKHBFRUJKIKMKH9RITQOQGLESBZGDDQLCYOUTCRFHJ9ARQYPQLRLOHDEAXEYTM9CCCRWGCJWRGAWYHSHBFXOPNKCVSHDCWPMTSZOUMNOTDWFEDLMSLLMWQBAVAPMPBGZ9HSJOPPFCRPDDUVBCYWKRAWFYVGWJEIQOTXUXSWCWKRQHBWSNZVIOIFFTSQNSOZYEQZJ9NQFERBTITOCPRUQAWAKWIM9MIRKNWVBGXQOIJANCKPOFGEBYCLYNPYMKVBZAYOMKUZJGEQFQIIZLJPZIZMQFDXOUJTAZUEDWZICGDLKCUVNEERXDPAMDEPLUC9NKOAJLQLVIIAVHYHGGJQEPVLKWSXPLZCRDILBKRCFLNLQWLOYEBABWRNQELYMKETIHEYAVDRBSFFVFGKAVZEWITNMBIHIVTHWLRKPTCRWG9FIU9WZJAVXMNMXQM9KACRFIKTSCVPDAZITVSFOOLFIRMBJKHHV9JLQBVRVBPYGCGJZBGVNKKGBMEKMFSNPQWVKAU99ZEXYRBFFOWYDJBAKE9HFYJCSLRXGZDWLMXA9AHZFLOZXTZKMKYAYZGKVMWUJANGYRQAFVTHMLCQNNRFLJSRHHYHIICFJZTNRMLYBKDOWMRWCPTUX9YGLWPSFQUBBTOEXLQGD9AHFL9QH9JCU99AQXCROUOTACDUMRPXVCGGLASE9LQSKP9SQBJGNNRRJFPESFWESFWJJRDVIVFTT9GKGANNXGVR9FXAENOIKLQ9KBEHSIWXTDMZEVBVHXXFBZ9XOFAIVVHIYNHDPPCBIWBNAAPWNYNAETHXWCZUFARSMSKANTRVBTAJUWLUBNXRBEMHSMYYCFJSOHIUJJGLBGTHJCPWJHUWWKJMPJWVUGCAXNVO9IHEUBXTQDOUHGLCRGNWIIHUTUQIPSDUSONGMGIU9IZUMRUEIIYZKFFQWLNYQCHPKBMQHECTHLSCUDBXO9BGMIKILFTDZMJFHDFWSZSFVQIOPBQUXQRXSMVYNJTGYV9LDGMNKELFOMAIGRXXNQYURLYSKYKCIFZPLYIBMCUARIRK9RAUUJAU9GJKLYMYOCYKCIWDAVFEKSRHSRSTW9IMHQEXZGWTZJYVRBMGLNZHSBBPQF9BPJSNJHYQC9XUSPGUVQZAURAPDEIIVEVNOW9NHQRWHNEWNZDFWRLRMKSKGSOQUSKROMGTOTSIMJUSSVNFBMAUFHOKOTYSEIMPFRCGJDI9NHLUUJHYMFY9RESEXFRMEPMMINDOKIBCNKGLBOLABOQXFWCUJYWQDRD9AADGBSCRCXERVQC9C9JOMVYQROULJTFTQVMVOSZ9YQIRPRQSTHSMNCNNZWHFWCLHVXADOU9N9QSFUIHIZXWJWWUMGQYZJTBKLXMZBCXPWBUZWDYJHUIRSVZTOAWV9EYTPCY9TTEKMIJQZSUVVCEDIHCRPTNWVEYNUUN9WXWQYIZUZJFETKSOKOT9CBFNKKRNJXZHHJGOWXHBULQMXMCNSLDHBGBMKHHMNOGFOTXZICOGGRJH9KZ9YDYZJCERMKLOCROVNGSZRKPHZJNAZQSTCUIYCYL9NS9RMKFIGKH9G9SRRZKLVDLNKU9PTEZGUEGGTKRJODNQYGEALTQOOBSOVCEBAZZWITGKMJUQODVKFDNTQSDKDYVBGUJNUIEDMSSOYLWVPAYGJH9ANL9VHGOAHXDKYPOWP9DGMUOBFZKHIMAHOBXMHMGNLVXZQDGYCVLZOEHOYNKKNLKPMAEPTUOBGRXXKBTCPKGNMSHXUDSWQOPRTLVJAFSEXDOMHENM9EMQJHL9DG9LRRMKGHMLTYX9S9EFOYHQLLGMJJEWF9KRYVNAGXFRZCRMCOVMXDYWWPUIKDYPASAECDPFSHFDTGTDNPXD9WZLNOIDLSD9TY9KUDUGVDTTAK9JQTJZU9KFLYD9VXGLBBGMEXASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXWMJLNY9999999999999999999999MINEIOTADOTCOM9999999999999X9DVQYD99C99999999E999999999BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZGFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999MINEIOTADOTCOM9999999999999DBNIZZXJE999999999L99999999XPBFUCTLEMOYKHMTZOKTNLTTHJY";

            // Act
            var transfer = new TransferItem() { Address = "ASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXW", Message = "TESTMESSAGE", Tag = "MINEIOTADOTCOM", Value = 0 };
            var tran = transfer.CreateTransactions()[0];
            tran.TrunkTransaction = "GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999";
            tran.BranchTransaction = "NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999";
            tran.Bundle = "9BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZ";
            tran.SetTimeStamp(new DateTime(2018, 01, 12, 0, 0, 0, DateTimeKind.Utc));

            var trytesOutput = tran.ToTransactionTrytes();

            // Assert
            Assert.AreEqual("ASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXW", tran.Address);
            Assert.AreEqual("0", tran.Value);
            Assert.AreEqual("MINEIOTADOTCOM9999999999999", tran.Tag);


            Assert.AreEqual("9BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZ", tran.Bundle);
            Assert.AreEqual("GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999", tran.TrunkTransaction);
            Assert.AreEqual("NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999", tran.BranchTransaction);

            // Assert.AreEqual("TESTMESSAGE999999999999999ACNTIBTNPLKKTRJHKAA9UAKFH9XCZTDPHIPCXNZ9KQIZCMJXEFUACJXWOZVZHOFYWJMSOSHJYHEBXUIG9TBDMVKSYAUDQAXSDDFPKNHOSETSRNCSSMMMLHGYDQLQWRQWBBOJSWMVXXYIOMI9EXTPJYIQ9GXCLZMKG9UNFKLKRGGJMIMMVGZGBDGRKWBHXCZOMCIRVLBMGXQKXEQKZNMVYHODWXJYYPVUQLLBGCILOYKHBFRUJKIKMKH9RITQOQGLESBZGDDQLCYOUTCRFHJ9ARQYPQLRLOHDEAXEYTM9CCCRWGCJWRGAWYHSHBFXOPNKCVSHDCWPMTSZOUMNOTDWFEDLMSLLMWQBAVAPMPBGZ9HSJOPPFCRPDDUVBCYWKRAWFYVGWJEIQOTXUXSWCWKRQHBWSNZVIOIFFTSQNSOZYEQZJ9NQFERBTITOCPRUQAWAKWIM9MIRKNWVBGXQOIJANCKPOFGEBYCLYNPYMKVBZAYOMKUZJGEQFQIIZLJPZIZMQFDXOUJTAZUEDWZICGDLKCUVNEERXDPAMDEPLUC9NKOAJLQLVIIAVHYHGGJQEPVLKWSXPLZCRDILBKRCFLNLQWLOYEBABWRNQELYMKETIHEYAVDRBSFFVFGKAVZEWITNMBIHIVTHWLRKPTCRWG9FIU9WZJAVXMNMXQM9KACRFIKTSCVPDAZITVSFOOLFIRMBJKHHV9JLQBVRVBPYGCGJZBGVNKKGBMEKMFSNPQWVKAU99ZEXYRBFFOWYDJBAKE9HFYJCSLRXGZDWLMXA9AHZFLOZXTZKMKYAYZGKVMWUJANGYRQAFVTHMLCQNNRFLJSRHHYHIICFJZTNRMLYBKDOWMRWCPTUX9YGLWPSFQUBBTOEXLQGD9AHFL9QH9JCU99AQXCROUOTACDUMRPXVCGGLASE9LQSKP9SQBJGNNRRJFPESFWESFWJJRDVIVFTT9GKGANNXGVR9FXAENOIKLQ9KBEHSIWXTDMZEVBVHXXFBZ9XOFAIVVHIYNHDPPCBIWBNAAPWNYNAETHXWCZUFARSMSKANTRVBTAJUWLUBNXRBEMHSMYYCFJSOHIUJJGLBGTHJCPWJHUWWKJMPJWVUGCAXNVO9IHEUBXTQDOUHGLCRGNWIIHUTUQIPSDUSONGMGIU9IZUMRUEIIYZKFFQWLNYQCHPKBMQHECTHLSCUDBXO9BGMIKILFTDZMJFHDFWSZSFVQIOPBQUXQRXSMVYNJTGYV9LDGMNKELFOMAIGRXXNQYURLYSKYKCIFZPLYIBMCUARIRK9RAUUJAU9GJKLYMYOCYKCIWDAVFEKSRHSRSTW9IMHQEXZGWTZJYVRBMGLNZHSBBPQF9BPJSNJHYQC9XUSPGUVQZAURAPDEIIVEVNOW9NHQRWHNEWNZDFWRLRMKSKGSOQUSKROMGTOTSIMJUSSVNFBMAUFHOKOTYSEIMPFRCGJDI9NHLUUJHYMFY9RESEXFRMEPMMINDOKIBCNKGLBOLABOQXFWCUJYWQDRD9AADGBSCRCXERVQC9C9JOMVYQROULJTFTQVMVOSZ9YQIRPRQSTHSMNCNNZWHFWCLHVXADOU9N9QSFUIHIZXWJWWUMGQYZJTBKLXMZBCXPWBUZWDYJHUIRSVZTOAWV9EYTPCY9TTEKMIJQZSUVVCEDIHCRPTNWVEYNUUN9WXWQYIZUZJFETKSOKOT9CBFNKKRNJXZHHJGOWXHBULQMXMCNSLDHBGBMKHHMNOGFOTXZICOGGRJH9KZ9YDYZJCERMKLOCROVNGSZRKPHZJNAZQSTCUIYCYL9NS9RMKFIGKH9G9SRRZKLVDLNKU9PTEZGUEGGTKRJODNQYGEALTQOOBSOVCEBAZZWITGKMJUQODVKFDNTQSDKDYVBGUJNUIEDMSSOYLWVPAYGJH9ANL9VHGOAHXDKYPOWP9DGMUOBFZKHIMAHOBXMHMGNLVXZQDGYCVLZOEHOYNKKNLKPMAEPTUOBGRXXKBTCPKGNMSHXUDSWQOPRTLVJAFSEXDOMHENM9EMQJHL9DG9LRRMKGHMLTYX9S9EFOYHQLLGMJJEWF9KRYVNAGXFRZCRMCOVMXDYWWPUIKDYPASAECDPFSHFDTGTDNPXD9WZLNOIDLSD9TY9KUDUGVDTTAK9JQTJZU9KFLYD9VXGLBBGMEX", tran.SignatureFragment);

            Assert.AreEqual("0", tran.LastIndex);
            Assert.AreEqual("0", tran.CurrentIndex);
            Assert.AreEqual(151575840, tran.Timestamp, tran.Timestamp);
            Assert.AreEqual("999999999999999999999999999", tran.Nonce); // Why is this 81 not 27?
            // Assert.AreEqual("EQK9ZETZMFMB9VMIEYEXKTAEMZBGQSDSAMCWXKZVVQXKZBUPGCJ9RRQLMWTEKLBWKNERC9ZMIROXZ9999", tran.Hash);
        }

        [TestMethod]
        public void Transaction_WithPOW_ToTrytes_SerialisesCorrectly()
        {
            // Arrange
            var trunk = "GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999";
            var branch = "NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999";            
            var transfer = new TransferItem() { Address = "ASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXW", Message = "TESTMESSAGE", Tag = "MINEIOTADOTCOM", Value = 0 };

            var trans = transfer.CreateTransactions();
            trans[0].SetAttachmentTime(new DateTime(2018, 01, 12, 0, 0, 0, DateTimeKind.Utc));
            trans[0].SetTimeStamp(new DateTime(2018, 01, 12, 0, 0, 0, DateTimeKind.Utc));
            var tranTrytes = trans.GetTrytes();

            // Act
            var trytesToSend = tranTrytes.DoPow(trunk, branch, 4, 1, CancellationToken.None).Result; // do the pow
            var tran = new TransactionItem(trytesToSend[0]);

            // Assert
            Assert.AreEqual("ASLIEIUJQHMVNHFQEXIDGGRRINZRHXZT9IWMZLRHWDYL9C9A9JSPBZUXJID9YERUQYRVALRVRLQZYSRXW", tran.Address);
            Assert.AreEqual("0", tran.Value);
            
            Assert.AreEqual(151575840, tran.Timestamp, tran.Timestamp);
            Assert.AreEqual(1515758400000, tran.AttachmentTimestamp, tran.AttachmentTimestamp);            
            Assert.AreEqual("0", tran.CurrentIndex);
            Assert.AreEqual("0", tran.LastIndex);
            Assert.AreEqual("GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999", tran.TrunkTransaction);
            Assert.AreEqual("NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999", tran.BranchTransaction);
            Assert.AreEqual("MINEIOTADOTCOM9999999999999", tran.Tag);
            // Assert.AreEqual("9BHGQLNJGFONXXJLSNEZXACIP9UQGV9HIWKP9X9COQLHBRSWEBSBJECCLLADKBUW9HAKSQZBNJZVIQAHZ", tran.Bundle);            
            // Assert.AreEqual("UIZGIWQZHBKTDRERGFGMSXC9XMU", tran.Nonce);
            // Assert.AreEqual("EQK9ZETZMFMB9VMIEYEXKTAEMZBGQSDSAMCWXKZVVQXKZBUPGCJ9RRQLMWTEKLBWKNERC9ZMIROXZ9999", tran.Hash);
        }

        [TestMethod]
        public void Transaction_CanSign_WithoutError()
        {
            var seed = "GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999";
            var address1 = Utils.IotaUtils.GenerateAddress(seed, 0);
            var address2 = Utils.IotaUtils.GenerateAddress(seed, 1);
            var pKey = Utils.Converter.ToTrytes(address1.PrivateKeyTrints);

            var signing = new Utils.Signing2(new Kerl());            
            var key1 = signing.key(Utils.Converter.ToTrits(seed), 0, 2);

            var address1again = Utils.IotaUtils.GenerateAddress(key1, false, CancellationToken.None);
            Assert.AreEqual(address1.Address, address1again);

            address1.Balance = 1;

            var trunk = "GFZIP9ZXJIJLUBYGTY9LGEDXVBLRCBEOEWYGZKHDLCCGXOVFPZM9MLQRMATDIICUFZRNXDNOBQIBA9999";
            var branch = "NYXCW9DAGWKXQXUL9OUIIJYJFDOBLEB9LGTNWMCNADXPJVGECZ9SKVRTOXSNNXRHPP9QMCXSWRLU99999";

            var transfer = new TransferItem() { Address = address2.Address, Message = "TESTMESSAGE", Tag = "MINEIOTADOTCOM", Value = 1 };

            // Act
            var trans = transfer.CreateTransactions(address1.Address, address1);

            // Assert

        }
    }
}
