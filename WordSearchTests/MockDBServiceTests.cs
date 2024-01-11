

using Services;

namespace WordSearchTests
{
    public class MockDBServiceTests
    {
        protected MockDBService _service;
        [SetUp]
        public void Setup()
        {
           _service = new MockDBService();
        }

        [Test]
        public void MockDataNotEmpty()
        {
            if (_service.Words.Count > 0)
            {
                Assert.Pass("��������� �� ������");
            }            
        }
        
        [Test]
        public void TestSearch()
        {
            const string mask = "�*�*�";
            const string searchGoodWord = "�����";
            const string searchBadWord = "�����";

            var result = _service.Search(mask);
            if (result.Contains(searchGoodWord) && !result.Contains(searchBadWord)) 
            { 
                Assert.Pass(); 
            }
            else { 
                Assert.Fail(); 
            }
        }
    }
}