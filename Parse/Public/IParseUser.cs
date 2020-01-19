using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Parse
{
	public interface IParseUser
	{
		/// <summary>
		/// Whether the ParseUser has been authenticated on this device. Only an authenticated
		/// ParseUser can be saved and deleted.
		/// </summary>
		bool IsAuthenticated { get; }

		string SessionToken { get; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		string Username { get; set; }

		/// <summary>
		/// Sets the password.
		/// </summary>
		string Password { set; }

		/// <summary>
		/// Sets the email address.
		/// </summary>
		string Email { get; set; }

		/// <summary>
		/// Gets whether the ParseObject has been fetched.
		/// </summary>
		bool IsDataAvailable { get; }

		/// <summary>
		/// Gets a set view of the keys contained in this object. This does not include createdAt,
		/// updatedAt, or objectId. It does include things like username and ACL.
		/// </summary>
		ICollection<string> Keys { get; }

		/// <summary>
		/// Gets or sets the ParseACL governing this object.
		/// </summary>
		ParseACL ACL { get; set; }

		/// <summary>
		/// Returns true if this object was created by the Parse server when the
		/// object might have already been there (e.g. in the case of a Facebook
		/// login)
		/// </summary>
#if !UNITY
#else
    internal
#endif
		bool IsNew
		{
			get;
#if !UNITY
		}

		/// <summary>
		/// Gets the last time this object was updated as the server sees it, so that if you make changes
		/// to a ParseObject, then wait a while, and then call <see cref="SaveAsync()"/>, the updated time
		/// will be the time of the <see cref="SaveAsync()"/> call rather than the time the object was
		/// changed locally.
		/// </summary>
		DateTime? UpdatedAt { get; }

		/// <summary>
		/// Gets the first time this object was saved as the server sees it, so that if you create a
		/// ParseObject, then wait a while, and then call <see cref="SaveAsync()"/>, the
		/// creation time will be the time of the first <see cref="SaveAsync()"/> call rather than
		/// the time the object was created locally.
		/// </summary>
		DateTime? CreatedAt { get; }

		/// <summary>
		/// Indicates whether this ParseObject has unsaved changes.
		/// </summary>
		bool IsDirty { get; }

		/// <summary>
		/// Gets or sets the object id. An object id is assigned as soon as an object is
		/// saved to the server. The combination of a <see cref="ClassName"/> and an
		/// <see cref="ObjectId"/> uniquely identifies an object in your application.
		/// </summary>
		string ObjectId { get; set; }

		/// <summary>
		/// Gets the class name for the ParseObject.
		/// </summary>
		string ClassName { get; }

		/// <summary>
		/// Removes a key from the object's data if it exists.
		/// </summary>
		/// <param name="key">The key to remove.</param>
		/// <exception cref="System.ArgumentException">Cannot remove the username key.</exception>
		void Remove(string key);

		/// <summary>
		/// Signs up a new user. This will create a new ParseUser on the server and will also persist the
		/// session on disk so that you can access the user using <see cref="CurrentUser"/>. A username and
		/// password must be set before calling SignUpAsync.
		/// </summary>
		Task SignUpAsync();

		/// <summary>
		/// Signs up a new user. This will create a new ParseUser on the server and will also persist the
		/// session on disk so that you can access the user using <see cref="CurrentUser"/>. A username and
		/// password must be set before calling SignUpAsync.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task SignUpAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Clears any changes to this object made since the last call to <see cref="SaveAsync()"/>.
		/// </summary>
		void Revert();

		/// <summary>
		/// Saves this object to the server.
		/// </summary>
		Task SaveAsync();

		/// <summary>
		/// Saves this object to the server.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task SaveAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Deletes this object on the server.
		/// </summary>
		Task DeleteAsync();

		/// <summary>
		/// Deletes this object on the server.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task DeleteAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets or sets a value on the object. It is recommended to name
		/// keys in partialCamelCaseLikeThis.
		/// </summary>
		/// <param name="key">The key for the object. Keys must be alphanumeric plus underscore
		/// and start with a letter.</param>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is
		/// retrieved and <paramref name="key"/> is not found.</exception>
		/// <returns>The value for the key.</returns>
		object this[string key] { get; set; }

		/// <summary>
		/// Atomically increments the given key by 1.
		/// </summary>
		/// <param name="key">The key to increment.</param>
		void Increment(string key);

		/// <summary>
		/// Atomically increments the given key by the given number.
		/// </summary>
		/// <param name="key">The key to increment.</param>
		/// <param name="amount">The amount to increment by.</param>
		void Increment(string key, long amount);

		/// <summary>
		/// Atomically increments the given key by the given number.
		/// </summary>
		/// <param name="key">The key to increment.</param>
		/// <param name="amount">The amount to increment by.</param>
		void Increment(string key, double amount);

		/// <summary>
		/// Atomically adds an object to the end of the list associated with the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The object to add.</param>
		void AddToList(string key, object value);

		/// <summary>
		/// Atomically adds objects to the end of the list associated with the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The objects to add.</param>
		void AddRangeToList<T>(string key, IEnumerable<T> values);

		/// <summary>
		/// Atomically adds an object to the end of the list associated with the given key,
		/// only if it is not already present in the list. The position of the insert is not
		/// guaranteed.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The object to add.</param>
		void AddUniqueToList(string key, object value);

		/// <summary>
		/// Atomically adds objects to the end of the list associated with the given key,
		/// only if they are not already present in the list. The position of the inserts are not
		/// guaranteed.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The objects to add.</param>
		void AddRangeUniqueToList<T>(string key, IEnumerable<T> values);

		/// <summary>
		/// Atomically removes all instances of the objects in <paramref name="values"/>
		/// from the list associated with the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The objects to remove.</param>
		void RemoveAllFromList<T>(string key, IEnumerable<T> values);

		/// <summary>
		/// Returns whether this object has a particular key.
		/// </summary>
		/// <param name="key">The key to check for</param>
		bool ContainsKey(string key);

		/// <summary>
		/// Gets a value for the key of a particular type.
		/// <typeparam name="T">The type to convert the value to. Supported types are
		/// ParseObject and its descendents, Parse types such as ParseRelation and ParseGeopoint,
		/// primitive types,IList&lt;T&gt;, IDictionary&lt;string, T&gt;, and strings.</typeparam>
		/// <param name="key">The key of the element to get.</param>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is
		/// retrieved and <paramref name="key"/> is not found.</exception>
		/// </summary>
		T Get<T>(string key);

		/// <summary>
		/// Access or create a Relation value for a key.
		/// </summary>
		/// <typeparam name="T">The type of object to create a relation for.</typeparam>
		/// <param name="key">The key for the relation field.</param>
		/// <returns>A ParseRelation for the key.</returns>
		ParseRelation<T> GetRelation<T>(string key) where T : ParseObject;

		/// <summary>
		/// Populates result with the value for the key, if possible.
		/// </summary>
		/// <typeparam name="T">The desired type for the value.</typeparam>
		/// <param name="key">The key to retrieve a value for.</param>
		/// <param name="result">The value for the given key, converted to the
		/// requested type, or null if unsuccessful.</param>
		/// <returns>true if the lookup and conversion succeeded, otherwise
		/// false.</returns>
		bool TryGetValue<T>(string key, out T result);

		/// <summary>
		/// A helper function for checking whether two ParseObjects point to
		/// the same object in the cloud.
		/// </summary>
		bool HasSameId(ParseObject other);

		/// <summary>
		/// Indicates whether key is unsaved for this ParseObject.
		/// </summary>
		/// <param name="key">The key to check for.</param>
		/// <returns><c>true</c> if the key has been altered and not saved yet, otherwise
		/// <c>false</c>.</returns>
		bool IsKeyDirty(string key);

		/// <summary>
		/// Adds a value for the given key, throwing an Exception if the key
		/// already has a value.
		/// </summary>
		/// <remarks>
		/// This allows you to use collection initialization syntax when creating ParseObjects,
		/// such as:
		/// <code>
		/// var obj = new ParseObject("MyType")
		/// {
		///     {"name", "foo"},
		///     {"count", 10},
		///     {"found", false}
		/// };
		/// </code>
		/// </remarks>
		/// <param name="key">The key for which a value should be set.</param>
		/// <param name="value">The value for the key.</param>
		void Add(string key, object value);

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		event PropertyChangedEventHandler PropertyChanged;

#endif
    }
}