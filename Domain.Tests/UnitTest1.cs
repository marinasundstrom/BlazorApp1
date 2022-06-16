namespace BlazorApp1.Domain.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var item = new Item("My Item", "Descr...", 0);
        }

        [Fact]
        public void Test2()
        {
            var item = new Item("My Item", "Descr...", 0);

            Comment comment = new Comment("Comment!");

            item.AddComment(comment);
        }
    }
}