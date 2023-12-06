Stack<string> stack = new Stack<string>();

Task producerTask = Task.Run(Producer);
Task consumerTask = Task.Run(Consumer);

await Task.WhenAll(producerTask, consumerTask);

Task Producer()
{
    while (true)
    {
        string? input = Console.ReadLine();

        lock (stack)
        {
            stack.Push(input ?? "EMPTY");
            Monitor.Pulse(stack);
        }
    }
}

Task Consumer()
{
    while (true)
    {
        lock (stack)
        {
            while (stack.Count == 0)
                Monitor.Wait(stack);
            
            string output = stack.Pop();

            Console.WriteLine($"Consumed: {output}");
        }
    }
}
