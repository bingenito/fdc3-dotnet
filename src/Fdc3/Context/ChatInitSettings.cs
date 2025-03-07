﻿/*
 * Morgan Stanley makes this available to you under the Apache License,
 * Version 2.0 (the "License"). You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0.
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Unless required by applicable law or agreed
 * to in writing, software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

namespace Finos.Fdc3.Context
{
    public class ChatInitSettings : Context, IContext
    {
        public ChatInitSettings(ContactList? members = null, Message? message = null, string? chatName = null, ChatInitSettingsOptions? options = null, object? id = null, string? name = null)
            : base(ContextTypes.ChatInitSettings, id, name)
        {
            this.Members = members;
            this.Message = message;
            this.ChatName = chatName;
            this.Options = options;
        }

        public string? ChatName { get; set; }
        public Message? Message { get; set; }
        public ContactList? Members { get; set; }
        public ChatInitSettingsOptions? Options { get; set; }

        object? IContext<object>.ID => base.ID;
    }

    public class ChatInitSettingsOptions
    {
        public bool? GroupRecipients { get; set; }
        public bool? IsPublic { get; set; }
        public bool? AllowHistoryBrowsing { get; set; }
        public bool? AllowMessageCopy { get; set; }
        public bool? AllowAddUser { get; set; }
    }
}
