﻿using System.ComponentModel.DataAnnotations;

namespace ChurchManager.Domain.Common
{
    public record Money
    {
        [MaxLength(5)]
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        // ORM required
        private Money(){}

        public Money(string currency, decimal amount)
        {
            if (amount <= 0) amount = 0;

            Currency = currency;
            Amount = amount;
        }
    }
}