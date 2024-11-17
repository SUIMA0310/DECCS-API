using System.Diagnostics.CodeAnalysis;

namespace Deccs.Api.Core.Model;

public readonly struct EventId(int value)
{
    public int Value { get; } = value;

    public static bool operator ==(EventId left, EventId right) => left.Value == right.Value;
    public static bool operator !=(EventId left, EventId right) => left.Value != right.Value;

    public static explicit operator int(EventId eventId) => eventId.Value;
    public static implicit operator EventId(int value) => new(value);

    public override bool Equals(object? obj)
    {
        if (obj is EventId other)
        {
            return Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
}

public readonly struct MarkdownText(string value)
{
    public string Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

    public static bool operator ==(MarkdownText left, MarkdownText right) => left.Value == right.Value;
    public static bool operator !=(MarkdownText left, MarkdownText right) => left.Value != right.Value;

    public static explicit operator string(MarkdownText markdownText) => markdownText.Value;
    public static implicit operator MarkdownText(string value) => new(value);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is MarkdownText other)
        {
            return Value == other.Value;
        }

        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}

public readonly struct EventDate
{
    public bool IsAllDay { get; }
    public DateTime Start { get; }
    public DateTime End { get; }

    public EventDate(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
        IsAllDay = start == end;
    }

    public EventDate(DateOnly date)
    {
        Start = date.ToDateTime(TimeOnly.MinValue);
        End = date.ToDateTime(TimeOnly.MinValue);
        IsAllDay = true;
    }

    public static bool operator ==(EventDate left, EventDate right) => left.Start == right.Start && left.End == right.End;
    public static bool operator !=(EventDate left, EventDate right) => left.Start != right.Start || left.End != right.End;

    public override bool Equals(object? obj)
    {
        if (obj is EventDate other)
        {
            return Start == other.Start && End == other.End;
        }
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Start, End);
}

public readonly struct EventLocation
{
    public string FieldName { get; }
    public string? Address { get; }
    public string? GoogleMapUrl { get; }

    public EventLocation(string fieldName, string address = null, string googleMapUrl = null)
    {
        FieldName = fieldName;
        Address = address;
        GoogleMapUrl = googleMapUrl;
    }

    public static bool operator ==(EventLocation left, EventLocation right) 
        => left.FieldName == right.FieldName &&
           left.Address == right.Address &&
           left.GoogleMapUrl == right.GoogleMapUrl;
    public static bool operator !=(EventLocation left, EventLocation right) 
        => left.FieldName == right.FieldName &&
           left.Address != right.Address &&
           left.GoogleMapUrl == right.GoogleMapUrl;

    public override bool Equals(object? obj)
    {
        if (obj is EventLocation other)
        {
            return FieldName == other.FieldName &&
                   Address == other.Address &&
                   GoogleMapUrl == other.GoogleMapUrl;
        }

        return false;
    }

    public override int GetHashCode() => Address.GetHashCode();

    public override string ToString() => Address;
}

public class DateOfEvent
{
    public bool IsFixed { get; private set; }

    public IReadOnlySet<EventDate> Dates { get; private set; }

    protected DateOfEvent(bool isFixed, IEnumerable<EventDate>? dates = null)
    {
        IsFixed = isFixed;
        Dates = dates?.ToHashSet() ?? new HashSet<EventDate>();
    }

    public void FixTheDate()
    {
        if (IsFixed) { return; }

        if (Dates.Count != 1)
        {
            throw new InvalidOperationException("The date cannot be fixed because there is not exactly one date.");
        }

        IsFixed = true;
    }

    public void SetDate(EventDate date)
    {
        if (IsFixed)
        {
            throw new InvalidOperationException("The date cannot be set because the date is fixed.");
        }

        Dates = new HashSet<EventDate> { date };
    }

    public void SetDate(IEnumerable<EventDate> dates)
    {
        if (IsFixed)
        {
            throw new InvalidOperationException("The date cannot be set because the date is fixed.");
        }

        Dates = dates.ToHashSet();
    }   

    public void AddDate(EventDate date)
    {
        if (IsFixed)
        {
            throw new InvalidOperationException("The date cannot be added because the date is fixed.");
        }

        ((HashSet<EventDate>)Dates).Add(date);
    }

    public void RemoveDate(EventDate date)
    {
        if (IsFixed)
        {
            throw new InvalidOperationException("The date cannot be removed because the date is fixed.");
        }

        ((HashSet<EventDate>)Dates).Remove(date);
    }
}

public enum EventStatus
{
    Draft,
    PrePublished,
    Published,
    Canceled
}

public class Event
{
    public static Event CreateDraft(EventId id, IEnumerable<User>? participant = null)
    {
    }

    protected Event(EventId id, string title, MarkdownText description, EventStatus status, DateOfEvent? date, EventLocation location, IEnumerable<User> )

    public EventId Id { get; }
    public string Title { get; private set; } = string.Empty;
    public MarkdownText Description { get; private set; } = new(string.Empty);
    public EventStatus Status { get; private set; }
    public DateOfEvent? Date { get; set; }
    public EventLocation? Location { get; set; }
    public IList<User> Participant { get; }
}

public class Reservation
{
    public int Id { get; }
    public required Event Event { get; set; }
    public required User User { get; set; }
    public required DateTime Date { get; set; }
}

public class User
{
    public string Id { get; }
    public required string NicName { get; set; }
    public required string Email { get; set; }
}

