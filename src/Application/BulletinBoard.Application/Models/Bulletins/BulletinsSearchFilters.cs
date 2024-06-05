﻿using BulletinBoard.Application.SearchFilters;

namespace BulletinBoard.Application.Models.Bulletins;

public record BulletinsSearchFilters(
    PageFilter Page,
    int? SearchNumber,
    string? SearchText,
    Guid? SearchUserId,
    BulletinsSortBy SortBy,
    bool Desc,
    BulletinsRatingFilter Rating,
    DateRangeFilters Created,
    DateRangeFilters Expiry);