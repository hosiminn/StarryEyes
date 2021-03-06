﻿using System;
using System.Collections.Generic;
using StarryEyes.Anomaly.TwitterApi.DataModels;

namespace StarryEyes.Filters.Expressions.Values.Users
{
    public sealed class User : ValueBase
    {
        public override IEnumerable<FilterExpressionType> SupportedTypes
        {
            get
            {
                yield return FilterExpressionType.Numeric;
                yield return FilterExpressionType.String;
            }
        }

        public override Func<TwitterStatus, long> GetNumericValueProvider()
        {
            return _ => _.GetOriginal().User.Id;
        }

        public override string GetNumericSqlQuery()
        {
            return "BaseUserId";
        }

        public override Func<TwitterStatus, string> GetStringValueProvider()
        {
            return _ => _.GetOriginal().User.ScreenName;
        }

        public override string GetStringSqlQuery()
        {
            return "(select ScreenName from User where Id = status.BaseUserId limit 1)";
        }

        public override string ToQuery()
        {
            return "user";
        }
    }

    public sealed class Retweeter : ValueBase
    {
        public override IEnumerable<FilterExpressionType> SupportedTypes
        {
            get
            {
                yield return FilterExpressionType.Numeric;
                yield return FilterExpressionType.String;
            }
        }

        public override Func<TwitterStatus, long> GetNumericValueProvider()
        {
            return _ => _.RetweetedOriginal != null ? _.User.Id : -1;
        }

        public override string GetNumericSqlQuery()
        {
            return "RetweeterId";
        }

        public override Func<TwitterStatus, string> GetStringValueProvider()
        {
            return _ => _.RetweetedOriginal != null ? _.User.ScreenName : String.Empty;
        }

        public override string GetStringSqlQuery()
        {
            return "(select ScreenName from User where Id = status.RetweeterId limit 1)";
        }

        public override string ToQuery()
        {
            return "retweeter";
        }
    }

    public sealed class StatusFavorites : ValueBase
    {
        public override IEnumerable<FilterExpressionType> SupportedTypes
        {
            get
            {
                yield return FilterExpressionType.Set;
                yield return FilterExpressionType.Numeric;
            }
        }

        public override Func<TwitterStatus, IReadOnlyCollection<long>> GetSetValueProvider()
        {
            return _ => _.FavoritedUsers ?? new long[0];
        }

        public override string GetSetSqlQuery()
        {
            return "(select UserId from Favorites where StatusId = status.Id)";
        }

        public override Func<TwitterStatus, long> GetNumericValueProvider()
        {
            return _ => (_.FavoritedUsers ?? new long[0]).Length;
        }

        public override string GetNumericSqlQuery()
        {
            return "(select count(Id) from Favorites where StatusId = status.Id)";
        }

        public override string ToQuery()
        {
            return "favs";
        }
    }

    public sealed class StatusRetweets : ValueBase
    {
        public override IEnumerable<FilterExpressionType> SupportedTypes
        {
            get
            {
                yield return FilterExpressionType.Set;
                yield return FilterExpressionType.Numeric;
            }
        }

        public override Func<TwitterStatus, IReadOnlyCollection<long>> GetSetValueProvider()
        {
            return _ => _.RetweetedUsers ?? new long[0];
        }

        public override string GetSetSqlQuery()
        {
            return "(select UserId from Retweets where StatusId = status.Id)";
        }

        public override Func<TwitterStatus, long> GetNumericValueProvider()
        {
            return _ => (_.RetweetedUsers ?? new long[0]).Length;
        }

        public override string GetNumericSqlQuery()
        {
            return "(select count(Id) from Retweets where StatusId = status.Id)";
        }

        public override string ToQuery()
        {
            return "retweets";
        }
    }

}
