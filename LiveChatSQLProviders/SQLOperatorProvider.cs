﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveChat.Entities;
using LiveChat.DAL;
using LiveChat.Providers;

namespace LiveChat.SQLProvider
{
    public class SQLOperatorProvider : OperatorProvider
    {
        private static void FillEntity(OperatorEntity newEntity, Operator existingEntity)
        {
            newEntity.EntityId = existingEntity.OperatorId;
            newEntity.Name = existingEntity.Name;
            newEntity.Password = existingEntity.Password;
            newEntity.Email = existingEntity.Email;
            newEntity.IsOnline = existingEntity.IsOnline;
            newEntity.IsAdmin = existingEntity.IsAdmin;
        }

        public override List<OperatorEntity> GetOnlineOperator()
        {
            List<OperatorEntity> results = new List<OperatorEntity>();
            OperatorEntity entity = null;
            foreach (var o in Operators.Fetch(true))
            {
                entity = new OperatorEntity();
                FillEntity(entity, o);
                results.Add(entity);
            }
            return results;
        }

        public override bool IsOperatorOnline()
        {
            return Operators.IsOperatorAvailable();
        }

        public override int Create(string name, string password, string email, bool isAdmin)
        {
            return Operators.Create(name, password, email, isAdmin);
        }

        public override void UpdateStatus(int operatorId, bool isOnline)
        {
            Operators.ChangeStatus(operatorId, isOnline);
        }

        public override OperatorEntity LogIn(string name, string password)
        {
            var op = Operators.LogIn(name, password);
            OperatorEntity o = new OperatorEntity();
            if (op != null)
            {
                FillEntity(o, op);
            }
            return o;
        }

        public override bool Remove(int operatorId)
        {
            return Operators.Remove(operatorId);
        }

        public override List<ChannelEntity> GetChatChannel(int operatorId)
        {
            List<ChannelEntity> results = new List<ChannelEntity>();
            if (Operators.HasNewRequests(operatorId))
            {
                ChannelEntity entity = null;
                foreach (Channel c in Channels.Fetch(operatorId))
                {
                    entity = new ChannelEntity();
                    entity.EntityId = c.ChannelId.ToString();
                    entity.RequestId = c.RequestId;
                    entity.OperatorId = c.OperatorId;
                    entity.OpenDate = c.OpenDate;
                    entity.AcceptDate = c.AcceptDate;
                    entity.CloseDate = c.CloseDate;
                    results.Add(entity);
                }
            }
            return results;
        }
    }
}
