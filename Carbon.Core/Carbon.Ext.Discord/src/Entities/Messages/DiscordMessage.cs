﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Oxide.Ext.Discord.Entities.Api;
using Oxide.Ext.Discord.Entities.Applications;
using Oxide.Ext.Discord.Entities.Channels;
using Oxide.Ext.Discord.Entities.Channels.Threads;
using Oxide.Ext.Discord.Entities.Emojis;
using Oxide.Ext.Discord.Entities.Guilds;
using Oxide.Ext.Discord.Entities.Interactions.MessageComponents;
using Oxide.Ext.Discord.Entities.Messages.Embeds;
using Oxide.Ext.Discord.Entities.Stickers;
using Oxide.Ext.Discord.Entities.Users;
using Oxide.Ext.Discord.Exceptions;
using Oxide.Ext.Discord.Helpers;
using Oxide.Ext.Discord.Helpers.Converters;
using Oxide.Ext.Discord.Interfaces;
using Oxide.Plugins;

namespace Oxide.Ext.Discord.Entities.Messages
{
    /// <summary>
    /// Represents a <a href="https://discord.com/developers/docs/resources/channel#message-object">Message Structure</a> sent in a channel within Discord..
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DiscordMessage : IFileAttachments
    {
        /// <summary>
        /// ID of the message
        /// </summary>
        [JsonProperty("id")]
        public Snowflake Id { get; set; }

        /// <summary>
        /// ID of the channel the message was sent in
        /// </summary>
        [JsonProperty("channel_id")]
        public Snowflake ChannelId { get; set; }

        /// <summary>
        /// ID of the guild the message was sent in
        /// </summary>
        [JsonProperty("guild_id")]
        public Snowflake? GuildId { get; set; }

        /// <summary>
        /// The author of this message (not guaranteed to be a valid user)
        /// The author object follows the structure of the user object, but is only a valid user in the case where the message is generated by a user or bot user.
        /// If the message is generated by a webhook, the author object corresponds to the webhook's id, username, and avatar.
        /// You can tell if a message is generated by a webhook by checking for the webhook_id on the message object.
        /// <see cref="DiscordUser"/>
        /// </summary>
        [JsonProperty("author")]
        public DiscordUser Author { get; set; }

        /// <summary>
        /// Member properties for this message's author
        /// <see cref="GuildMember"/>
        /// </summary>
        [JsonProperty("member")]
        public GuildMember Member { get; set; }
        
        /// <summary>
        /// Contents of the message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// When this message was sent
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// When this message was edited (or null if never)
        /// </summary>
        [JsonProperty("edited_timestamp")]
        public DateTime? EditedTimestamp { get; set; }
        
        /// <summary>
        /// Whether this was a TTS message
        /// </summary>
        [JsonProperty("tts")]
        public bool Tts { get; set; }

        /// <summary>
        /// Whether this message mentions everyone
        /// </summary>
        [JsonProperty("mention_everyone")]
        public bool MentionEveryone { get; set; }

        /// <summary>
        /// Users specifically mentioned in the message
        /// <see cref="DiscordUser"/>
        /// </summary>
        [JsonConverter(typeof(HashListConverter<DiscordUser>))]
        [JsonProperty("mentions")]
        public Hash<Snowflake, DiscordUser> Mentions { get; set; }

        /// <summary>
        /// Roles specifically mentioned in this message
        /// </summary>
        [JsonProperty("mention_roles")]
        public List<Snowflake> MentionRoles { get; set; }
        
        /// <summary>
        /// Channels specifically mentioned in this message
        /// <see cref="ChannelMention"/>
        /// </summary>
        [JsonConverter(typeof(HashListConverter<ChannelMention>))]
        [JsonProperty("mention_channels")]
        public Hash<Snowflake, ChannelMention> MentionsChannels { get; set; }

        /// <summary>
        /// Any attached files
        /// <see cref="MessageAttachment"/>
        /// </summary>
        [JsonConverter(typeof(HashListConverter<MessageAttachment>))]
        [JsonProperty("attachments")]
        public Hash<Snowflake, MessageAttachment> Attachments { get; set; }

        /// <summary>
        /// Any embedded content
        /// <see cref="Embeds"/>
        /// </summary>
        [JsonProperty("embeds")]
        public List<DiscordEmbed> Embeds { get; set; }

        /// <summary>
        /// Reactions to the message
        /// <see cref="MessageReaction"/>
        /// </summary>
        [JsonProperty("reactions")]
        public List<MessageReaction> Reactions { get; set; }
        
        /// <summary>
        /// Used for validating a message was sent
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Whether this message is pinned
        /// </summary>
        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        /// <summary>
        /// If the message is generated by a webhook, this is the webhook's id
        /// </summary>
        [JsonProperty("webhook_id")]
        public Snowflake? WebhookId { get; set; }

        /// <summary>
        /// Type of message
        /// <see cref="MessageType"/>
        /// </summary>
        [JsonProperty("type")]
        public MessageType? Type { get; set; }
        
        /// <summary>
        /// Sent with Rich Presence-related chat embeds
        /// <see cref="MessageActivity"/>
        /// </summary>
        [JsonProperty("activity")]
        public MessageActivity Activity { get; set; }
        
        /// <summary>
        /// Sent with Rich Presence-related chat embeds
        /// <see cref="Application"/>
        /// </summary>
        [JsonProperty("application")]
        public DiscordApplication Application { get; set; }
        
        /// <summary>
        /// If the message is an Interaction or application-owned webhook, this is the id of the application
        /// </summary>
        [JsonProperty("application_id")]
        public Snowflake? ApplicationId { get; set; }

        /// <summary>
        /// Data showing the source of a crosspost, channel follow add, pin, or reply message
        /// <see cref="MessageReference"/>
        /// </summary>
        [JsonProperty("message_reference")]
        public MessageReference MessageReference { get; set; }
        
        /// <summary>
        /// Message flags combined as a bitfield
        /// <see cref="MessageFlags"/>
        /// </summary>
        [JsonProperty("flags")]
        public MessageFlags Flags { get; set; }

        /// <summary>
        /// The message associated with the message_reference
        /// </summary>
        [JsonProperty("referenced_message")]
        public DiscordMessage ReferencedMessage { get; internal set; }
        
        /// <summary>
        /// Sent if the message is a response to an Interaction
        /// </summary>
        [JsonProperty("interaction")]
        public MessageInteraction Interaction { get; set; }
        
        /// <summary>
        /// The thread that was started from this message, includes thread member object
        /// </summary>
        [JsonProperty("thread")]
        public DiscordChannel Thread { get; set; }
        
        /// <summary>
        /// Sent if the message contains components like buttons, action rows, or other interactive components
        /// </summary>
        [JsonProperty("components")]
        public List<ActionRowComponent> Components { get; set; }
        
        /// <summary>
        /// Sent if the message contains stickers
        /// </summary>
        [JsonConverter(typeof(HashListConverter<DiscordSticker>))]
        [JsonProperty("sticker_items")]
        public Hash<Snowflake, DiscordSticker> StickerItems { get; set; }
        
        /// <summary>
        /// File Attachments to add to the message on edit
        /// </summary>
        public List<MessageFileAttachment> FileAttachments { get; set; }
        
        /// <summary>
        /// Post a message to a guild text or DM channel.
        /// If operating on a guild channel, this endpoint requires the SEND_MESSAGES permission to be present on the current user.
        /// If the tts field is set to true, the SEND_TTS_MESSAGES permission is required for the message to be spoken.
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="channelId">Channel ID to send the message to</param>
        /// <param name="message">Message to be created</param>
        /// <param name="callback">Callback with the created message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public static void CreateMessage(DiscordClient client, Snowflake channelId, MessageCreate message, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!channelId.IsValid()) throw new InvalidSnowflakeException(nameof(channelId));
            message.Validate();
            message.ValidateChannelMessage();
            client.Bot.Rest.DoRequest($"/channels/{channelId}/messages", RequestMethod.POST, message, callback, error);
        }

        /// <summary>
        /// Post a message to a guild text or DM channel.
        /// If operating on a guild channel, this endpoint requires the SEND_MESSAGES permission to be present on the current user.
        /// If the tts field is set to true, the SEND_TTS_MESSAGES permission is required for the message to be spoken.
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="channelId">Channel ID to send the message to</param>
        /// <param name="message">Content of the message</param>
        /// <param name="callback">Callback with the created message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public static void CreateMessage(DiscordClient client, Snowflake channelId, string message, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!channelId.IsValid()) throw new InvalidSnowflakeException(nameof(channelId));
            MessageCreate createMessage = new MessageCreate
            {
                Content = message
            };

            CreateMessage(client, channelId, createMessage, callback, error);
        }

        /// <summary>
        /// Post a message to a guild text or DM channel.
        /// If operating on a guild channel, this endpoint requires the SEND_MESSAGES permission to be present on the current user.
        /// If the tts field is set to true, the SEND_TTS_MESSAGES permission is required for the message to be spoken.
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="channelId">Channel ID to send the message to</param>
        /// <param name="embed">Embed to be send in the message</param>
        /// <param name="callback">Callback with the created message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public static void CreateMessage(DiscordClient client, Snowflake channelId, DiscordEmbed embed, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!channelId.IsValid()) throw new InvalidSnowflakeException(nameof(channelId));
            MessageCreate createMessage = new MessageCreate
            {
                Embeds = new List<DiscordEmbed> {embed}
            };

            CreateMessage(client, channelId, createMessage, callback, error);
        }
        
        /// <summary>
        /// Post a message to a guild text or DM channel.
        /// If operating on a guild channel, this endpoint requires the SEND_MESSAGES permission to be present on the current user.
        /// If the tts field is set to true, the SEND_TTS_MESSAGES permission is required for the message to be spoken.
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="channelId">Channel ID to send the message to</param>
        /// <param name="embeds">Embeds to be send in the message</param>
        /// <param name="callback">Callback with the created message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public static void CreateMessage(DiscordClient client, Snowflake channelId, List<DiscordEmbed> embeds, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!channelId.IsValid()) throw new InvalidSnowflakeException(nameof(channelId));
            MessageCreate createMessage = new MessageCreate
            {
                Embeds = embeds
            };

            CreateMessage(client, channelId, createMessage, callback, error);
        }

        /// <summary>
        /// Returns a specific message in the channel.
        /// If operating on a guild channel, this endpoint requires the 'READ_MESSAGE_HISTORY' permission to be present on the current user.
        /// See <a href="https://discord.com/developers/docs/resources/channel#get-channel-message">Get Channel Messages</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="channelId">Channel ID where the message is</param>
        /// <param name="messageId">Message ID of the message</param>
        /// <param name="callback">Callback with the message for the ID</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public static void GetChannelMessage(DiscordClient client, Snowflake channelId, Snowflake messageId, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!channelId.IsValid()) throw new InvalidSnowflakeException(nameof(channelId));
            if (!messageId.IsValid()) throw new InvalidSnowflakeException(nameof(messageId));
            client.Bot.Rest.DoRequest($"/channels/{channelId}/messages/{messageId}", RequestMethod.GET, null, callback, error);
        }
        
        /// <summary>
        /// Replies to a previously sent message
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="message">Message to send</param>
        /// <param name="callback">Callback with the message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void Reply(DiscordClient client, MessageCreate message, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (message.MessageReference == null)
            {
                message.MessageReference = new MessageReference {MessageId = Id, GuildId = GuildId};
            }
            
            message.Validate();
            message.ValidateChannelMessage();
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages", RequestMethod.POST, message, callback, error);
        }
        
        /// <summary>
        /// Replies to a previously sent message
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="message">Message text to send</param>
        /// <param name="callback">Callback with the message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void Reply(DiscordClient client, string message, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            MessageCreate newMessage = new MessageCreate
            {
                Content = message
            };

            Reply(client, newMessage, callback, error);
        }

        /// <summary>
        /// Replies to a previously sent message
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="embed">Embed to send</param>
        /// <param name="callback">Callback with the message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void Reply(DiscordClient client, DiscordEmbed embed, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            Reply(client, new List<DiscordEmbed> {embed}, callback, error);
        }
        
        /// <summary>
        /// Replies to a previously sent message
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-message">Create Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="embeds">Embeds to send</param>
        /// <param name="callback">Callback with the message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void Reply(DiscordClient client, List<DiscordEmbed> embeds, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            MessageCreate newMessage = new MessageCreate
            {
                Embeds = embeds,
            };

            Reply(client, newMessage, callback, error);
        }
        
        /// <summary>
        /// Crosspost a message in a News Channel to following channels.
        /// This endpoint requires the 'SEND_MESSAGES' permission, if the current user sent the message, or additionally the 'MANAGE_MESSAGES' permission, for all other messages, to be present for the current user.
        /// See <a href="https://discord.com/developers/docs/resources/channel#crosspost-message">Crosspost Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="messageId">Message ID to cross post</param>
        /// <param name="callback">Callback with the cross posted message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void CrossPostMessage(DiscordClient client, Snowflake messageId, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            if (!messageId.IsValid()) throw new InvalidSnowflakeException(nameof(messageId));
            client.Bot.Rest.DoRequest($"/channels/{Id}/messages/{messageId}/crosspost", RequestMethod.POST, null, callback, error);
        }
        
        /// <summary>
        /// Crosspost a message in a News Channel to following channels.
        /// This endpoint requires the 'SEND_MESSAGES' permission, if the current user sent the message, or additionally the 'MANAGE_MESSAGES' permission, for all other messages, to be present for the current user.
        /// See <a href="https://discord.com/developers/docs/resources/channel#crosspost-message">Crosspost Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="message">Message to cross post</param>
        /// <param name="callback">Callback with the cross posted message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void CrossPostMessage(DiscordClient client, DiscordMessage message, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            CrossPostMessage(client, message.Id, callback, error);
        }

        /// <summary>
        /// Create a reaction for the message.
        /// This endpoint requires the 'READ_MESSAGE_HISTORY' permission to be present on the current user.
        /// Additionally, if nobody else has reacted to the message using this emoji, this endpoint requires the 'ADD_REACTIONS' permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-reaction">Create Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to react with.</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void CreateReaction(DiscordClient client, DiscordEmoji emoji, Action callback = null, Action<RestError> error = null)
        {
            CreateReaction(client, emoji.ToDataString(), callback, error);
        }
        
        /// <summary>
        /// Create a reaction for the message.
        /// This endpoint requires the 'READ_MESSAGE_HISTORY' permission to be present on the current user.
        /// Additionally, if nobody else has reacted to the message using this emoji, this endpoint requires the 'ADD_REACTIONS' permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#create-reaction">Create Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to react with.</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void CreateReaction(DiscordClient client, string emoji, Action callback = null, Action<RestError> error = null)
        {
            Validation.ValidateEmoji(emoji);
            
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions/{emoji}/@me", RequestMethod.PUT, null, callback, error);
        }

        /// <summary>
        /// Delete a reaction the current user has made for the message
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-own-reaction">Delete Own Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteOwnReaction(DiscordClient client, DiscordEmoji emoji, Action callback = null, Action<RestError> error = null)
        {
            DeleteOwnReaction(client, emoji.ToDataString(), callback, error);
        }
        
        /// <summary>
        /// Delete a reaction the current user has made for the message
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-own-reaction">Delete Own Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteOwnReaction(DiscordClient client, string emoji, Action callback = null, Action<RestError> error = null)
        {
            Validation.ValidateEmoji(emoji);
            
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions/{emoji}/@me", RequestMethod.DELETE, null, callback, error);
        }

        /// <summary>
        /// Deletes another user's reaction.
        /// This endpoint requires the 'MANAGE_MESSAGES' permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-user-reaction">Delete User Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete</param>
        /// <param name="userId">User ID who add the reaction</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteUserReaction(DiscordClient client, DiscordEmoji emoji, Snowflake userId, Action callback = null, Action<RestError> error = null)
        {
            if (!userId.IsValid()) throw new InvalidSnowflakeException(nameof(userId));
            DeleteUserReaction(client, emoji.ToDataString(), userId, callback, error);
        }

        /// <summary>
        /// Deletes another user's reaction.
        /// This endpoint requires the 'MANAGE_MESSAGES' permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-user-reaction">Delete User Reaction</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete</param>
        /// <param name="userId">User ID who add the reaction</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteUserReaction(DiscordClient client, string emoji, Snowflake userId, Action callback = null, Action<RestError> error = null)
        {
            if (!userId.IsValid()) throw new InvalidSnowflakeException(nameof(userId));
            
            Validation.ValidateEmoji(emoji);
            
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions/{emoji}/{userId}", RequestMethod.DELETE, null, callback, error);
        }

        /// <summary>
        /// Get a list of users that reacted with this emoji
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#get-reactions">Get Reactions</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to get the list for</param>
        /// <param name="callback">Callback with a list of users who reacted</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void GetReactions(DiscordClient client, DiscordEmoji emoji, Action<List<DiscordUser>> callback = null, Action<RestError> error = null)
        {
            GetReactions(client, emoji.ToDataString(), callback, error);
        }
        
        /// <summary>
        /// Get a list of users that reacted with this emoji
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#get-reactions">Get Reactions</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to get the list for</param>
        /// <param name="callback">Callback with a list of users who reacted</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void GetReactions(DiscordClient client, string emoji, Action<List<DiscordUser>> callback = null, Action<RestError> error = null)
        {
            Validation.ValidateEmoji(emoji);

            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions/{emoji}", RequestMethod.GET, null, callback, error);
        }

        /// <summary>
        /// Deletes all reactions on a message.
        /// This endpoint requires the 'MANAGE_MESSAGES' permission to be present on the current user.
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteAllReactions(DiscordClient client, Action callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions", RequestMethod.DELETE, null, callback, error);
        }
        
        /// <summary>
        /// Deletes all the reactions for a given emoji on a message.
        /// This endpoint requires the MANAGE_MESSAGES permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-all-reactions-for-emoji">Delete All Reactions for Emoji</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete all reactions for</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteAllReactionsForEmoji(DiscordClient client, DiscordEmoji emoji, Action callback = null, Action<RestError> error = null)
        {
            DeleteAllReactionsForEmoji(client, emoji.ToDataString(), callback, error);
        }
        
        /// <summary>
        /// Deletes all the reactions for a given emoji on a message.
        /// This endpoint requires the MANAGE_MESSAGES permission to be present on the current user.
        /// Valid emoji formats are the unicode emoji character '😀' or for custom emoji format must be &lt;emojiName:emojiId&gt;
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-all-reactions-for-emoji">Delete All Reactions for Emoji</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="emoji">Emoji to delete all reactions for</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteAllReactionsForEmoji(DiscordClient client, string emoji, Action callback = null, Action<RestError> error = null)
        {
            Validation.ValidateEmoji(emoji);
            
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/reactions/{emoji}", RequestMethod.DELETE, null, callback, error);
        }

        /// <summary>
        /// Edit a previously sent message.
        /// The fields content, embed, allowed_mentions and flags can be edited by the original message author.
        /// Other users can only edit flags and only if they have the MANAGE_MESSAGES permission in the corresponding channel.
        /// When specifying flags, ensure to include all previously set flags/bits in addition to ones that you are modifying.
        /// Only flags documented in the table below may be modified by users (unsupported flag changes are currently ignored without error).
        /// See <a href="https://discord.com/developers/docs/resources/channel#edit-message">Edit Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="callback">Callback with the updated message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void EditMessage(DiscordClient client, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}", RequestMethod.PATCH, this, callback, error);
        }

        /// <summary>
        /// Delete a message.
        /// If operating on a guild channel and trying to delete a message that was not sent by the current user, this endpoint requires the MANAGE_MESSAGES permission.
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-message">Delete Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="callback">Callback with the delete message</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void DeleteMessage(DiscordClient client, Action<DiscordMessage> callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}", RequestMethod.DELETE, null, callback, error);
        }

        /// <summary>
        /// Pin a message in a channel.
        /// Requires the MANAGE_MESSAGES permission.
        /// See <a href="https://discord.com/developers/docs/resources/channel#add-pinned-channel-message">Add Pinned Channel Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="callback">Callback when the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void PinMessage(DiscordClient client, Action callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/pins/{Id}", RequestMethod.PUT, null, callback, error);
        }

        /// <summary>
        /// Delete a pinned message in a channel.
        /// Requires the MANAGE_MESSAGES permission.
        /// See <a href="https://discord.com/developers/docs/resources/channel#delete-pinned-channel-message">Unpin Pinned Channel Message</a>
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="callback">Callback once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        public void UnpinMessage(DiscordClient client, Action callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/pins/{Id}", RequestMethod.DELETE, null, callback, error);
        }
        
        /// <summary>
        /// Creates a new public thread this message
        /// </summary>
        /// <param name="client">Client to use</param>
        /// <param name="create">Data to use when creating the thread</param>
        /// <param name="callback">Callback with the thread once the action is completed</param>
        /// <param name="error">Callback when an error occurs with error information</param>
        /// See <a href="https://discord.com/developers/docs/resources/channel#start-thread-with-message"></a>
        public void StartPublicThread(DiscordClient client, ThreadCreate create, Action<DiscordChannel> callback = null, Action<RestError> error = null)
        {
            client.Bot.Rest.DoRequest($"/channels/{ChannelId}/messages/{Id}/threads", RequestMethod.POST, create, callback, error);
        }
    }
}
