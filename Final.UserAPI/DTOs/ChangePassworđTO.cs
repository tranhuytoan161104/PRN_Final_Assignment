﻿namespace Final.UserAPI.DTOs
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}