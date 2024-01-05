

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
                Assert.Pass("Коллекция не пустая");
            }            
        }
        [Test]
        public void TestAddingNewWords()
        {
            const string newWord = "слово";
            int prevCount = _service.Words.Count;
            _service.Add(newWord);
            int currCount = _service.Words.Count;  
            if (currCount == (prevCount + 1) && _service.Words[_service.Words.Count-1] == newWord) {
                Assert.Pass();
            }
        }
        [Test]
        public void TestSearch()
        {
            const string mask = "б*к*с";
            const string searchGoodWord = "бекас";
            const string searchBadWord = "слово";

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