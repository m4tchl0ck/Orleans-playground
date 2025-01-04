namespace Cqrs;

public interface IGrainCommandHandler<TGrainCommand> : IGrainWithStringKey,
    ICommandHandler<TGrainCommand>
    where TGrainCommand : IGrainCommand;
