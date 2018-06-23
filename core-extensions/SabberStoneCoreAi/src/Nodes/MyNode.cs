using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;

namespace SabberStoneCoreAi.src.Nodes
{
    class MyNode
    {
		public int stateValue = 0;
		public int numVisits = 0;
		public SabberStoneCoreAi.POGame.POGame game = null;
		public MyNode parent = null;
		public List<MyNode> children = null;
		public MyNode(MyNode parent, SabberStoneCoreAi.POGame.POGame game)
		{
			this.parent = parent;
			this.game = game;
		}
		public MyNode findRoot()
		{
			if(parent == null)
			{
				return this;
			}
			return parent.findRoot();
		}
		public void backPropagate()
		{
			PlayState ps = game.CurrentPlayer.PlayState;
		}
    }
}
