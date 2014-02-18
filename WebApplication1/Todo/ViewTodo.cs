using System.Collections.Generic;
using ServiceStack;

namespace WebApplication1.Todo
{
    [DefaultView("ViewTodos")]
    [Route("/todos")]
    public class ViewTodo : IReturn<ViewTodoResponse>
    {
         
    }

    public class ViewTodoResponse
    {
        public List<string> Todos { get; set; }
    }

    public class TodoService : Service
    {
        public ViewTodoResponse Get(ViewTodo request)
        {
            return new ViewTodoResponse()
            {
                Todos = new List<string> { "Do laundry", "Pickup kids", "Sleep"}
            };
        }
    }
}