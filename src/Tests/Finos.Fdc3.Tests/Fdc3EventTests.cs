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

namespace Finos.Fdc3.Tests;

public class Fdc3EventTests
{
    [Fact]
    public void Fdc3Event_PropertiesMatchParams()
    {
        IFdc3Event fdc3Event = new Fdc3Event("type", new Fdc3ChannelChangedEventDetails("channelid"));
        Assert.Same("type", fdc3Event.Type);
        Assert.IsType<Fdc3ChannelChangedEventDetails>(fdc3Event.Details);
        Assert.Same("channelid", ((Fdc3ChannelChangedEventDetails)fdc3Event.Details).CurrentChannelId);

        fdc3Event = new Fdc3Event("type", null);
        Assert.Null(fdc3Event.Details);
    }

    [Fact]
    public void Fdc3ChannelChangedEventDetails_PropertiesMatchParams()
    {
        IFdc3ChannelChangedEventDetails details = new Fdc3ChannelChangedEventDetails("channelid");
        Assert.Same("channelid", details.CurrentChannelId);

        details = new Fdc3ChannelChangedEventDetails(null);
        Assert.Null(details.CurrentChannelId);
    }

    [Fact]
    public void Fdc3ChannelChangedEvent_PropertiesMatchParams()
    {
        IFdc3Event fdc3Event = new Fdc3ChannelChangedEvent("channelid");
        Assert.Same(Fdc3EventType.UserChannelChanged, fdc3Event.Type);
        Assert.IsType<Fdc3ChannelChangedEventDetails>(fdc3Event.Details);
        Assert.Same("channelid", ((Fdc3ChannelChangedEventDetails)fdc3Event.Details).CurrentChannelId);

        fdc3Event = new Fdc3ChannelChangedEvent(null);
        Fdc3ChannelChangedEventDetails? details = fdc3Event.Details as Fdc3ChannelChangedEventDetails;
        Assert.Null(details?.CurrentChannelId);
    }

    [Fact]
    public void Fdc3PrivateChannelEventDetails_PropertiesMatchParams()
    {
        IFdc3PrivateChannelEventDetails details = new Fdc3PrivateChannelEventDetails("contexttype");
        Assert.Same("contexttype", details.ContextType);

        details = new Fdc3PrivateChannelEventDetails(null);
        Assert.Null(details.ContextType);
    }

    [Fact]
    public void Fdc3PrivateChannelAddContextListenerEvent_PropertiesMatchParams()
    {
        IFdc3Event fdc3Event = new Fdc3PrivateChannelAddContextListenerEvent("contextType");
        Assert.Same(Fdc3PrivateChannelEventType.AddContextListener, fdc3Event.Type);

        IFdc3PrivateChannelEventDetails? details = fdc3Event.Details as IFdc3PrivateChannelEventDetails;
        Assert.Same("contextType", details?.ContextType);

    }

    [Fact]
    public void Fdc3PrivateChannelUnsubscribeListenerEvent_PropertiesMatchParams()
    {
        IFdc3Event fdc3Event = new Fdc3PrivateChannelUnsubscribeListenerEvent("contextType");
        Assert.Same(Fdc3PrivateChannelEventType.Unsubscribe, fdc3Event.Type);

        IFdc3PrivateChannelEventDetails? details = fdc3Event.Details as IFdc3PrivateChannelEventDetails;
        Assert.Same("contextType", details?.ContextType);
    }

    [Fact]
    public void Fdc3PrivateChanneDisconnectEvent_PropertiesMatchParams()
    {
        IFdc3Event fdc3Event = new Fdc3PrivateChanneDisconnectEvent();
        Assert.Same(Fdc3PrivateChannelEventType.Disconnect, fdc3Event.Type);
        Assert.Null(fdc3Event.Details);
    }
}