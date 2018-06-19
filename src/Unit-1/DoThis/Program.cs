using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            // YOU NEED TO FILL IN HERE
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            //Props consoleWriterProps = Props.Create(typeof(ConsoleWriterActor));
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props stringValidationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            IActorRef stringValidationActor = MyActorSystem.ActorOf(stringValidationActorProps, "stringValidationActor");

            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            // pass tailCoordinatorActor to fileValidatorActorProps (just adding one extra arg)
            Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor/*, tailCoordinatorActor*/));
            IActorRef fileValidationActor = MyActorSystem.ActorOf(fileValidatorActorProps, "fileValidationActor");

            //Props consoleReaderProps = Props.Create<ConsoleReaderActor>(stringValidationActor);
            //Props consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidationActor);
            Props consoleReaderProps = Props.Create<ConsoleReaderActor>();
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
