﻿namespace RequestChallange.Api
{
    // StringMessage - строковое сообщение
    public record StringMessage(string Message);
    // ErrorMessage - сообщение об ошибке
    public record ErrorMessage(string Type, string Message);
}
